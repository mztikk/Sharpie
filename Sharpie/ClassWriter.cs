using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpie
{
    public class ClassWriter : BaseWriter
    {
        private readonly HashSet<string> _baseClasses = new HashSet<string>();

        private readonly List<Field> _fields = new List<Field>();

        public readonly string ClassName;

        public readonly UsingWriter Usings;

        public readonly NamespaceWriter Namespace;

        public ClassWriter(IndentedStreamWriter writer, string className) : base(writer)
        {
            Usings = new UsingWriter(writer);
            Namespace = new NamespaceWriter(writer)
            {
                Namespace = className
            };

            ClassName = className;
        }

        public Accessibility Accessibility { get; set; } = Accessibility.Public;

        public void AddBaseClass(string name) => _baseClasses.Add(name);

        private string GetInheritance()
        {
            if (_baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", _baseClasses);
        }

        public static IEnumerable<string> GetLines(string s)
        {
            using (StringReader reader = new StringReader(s))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line is null)
                    {
                        break;
                    }

                    yield return line;
                }
            }
        }

        public static string GetCSharpTypeName(string s) => s switch
        {
            "SByte" => "sbyte",
            "Byte" => "byte",
            "Int16" => "short",
            "UInt16" => "ushort",
            "Int32" => "int",
            "UInt32" => "uint",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Single" => "float",
            "Double" => "double",
            "Boolean" => "bool",
            "Char" => "char",
            "String" => "string",
            "Object" => "object",
            _ => s,
        };

        public override async Task Begin()
        {
            await Usings.Run().ConfigureAwait(false);
            await Namespace.Begin().ConfigureAwait(false);
            await WriteLine(Accessibility.ToSharpieString() + " class " + ClassName + GetInheritance()).ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        public override async Task End()
        {
            foreach (Field field in _fields)
            {
                await WriteLine(field.ToString()).ConfigureAwait(false);
            }

            if (_fields.Count > 0)
            {
                await WriteLine().ConfigureAwait(false);
            }

            IndentationLevel--;
            await WriteLine("}").ConfigureAwait(false);
            await Namespace.End().ConfigureAwait(false);
        }

        public virtual async Task AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> body)
        {
            await WriteLine(accessibility.ToSharpieString() + " " + ClassName + "(" + string.Join(", ", arguments) + ")").ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);

            IndentationLevel++;
            foreach (string line in body)
            {
                await WriteLine(line).ConfigureAwait(false);
            }
            IndentationLevel--;

            await WriteLine("}").ConfigureAwait(false);
            await WriteLine().ConfigureAwait(false);
        }

        public virtual async Task AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, string body) => await AddConstructor(accessibility, arguments, GetLines(body)).ConfigureAwait(false);

        public virtual async Task AddConstructor(Accessibility accessibility = Accessibility.Public) => await AddConstructor(accessibility, Enumerable.Empty<Argument>(), Enumerable.Empty<string>()).ConfigureAwait(false);

        public void AddField(Accessibility accessibility, bool readOnly, string type, string name, string? initialValue = null) => _fields.Add(new Field(accessibility, readOnly, type, name, initialValue));

        public void AddField(Accessibility accessibility, string type, string name) => AddField(accessibility, false, type, name);

        public void AddField<T>(Accessibility accessibility, bool readOnly, string name) => AddField(accessibility, readOnly, GetCSharpTypeName(typeof(T).Name), name);

        public void AddField<T>(Accessibility accessibility, string name) => AddField(accessibility, GetCSharpTypeName(typeof(T).Name), name);
    }
}
