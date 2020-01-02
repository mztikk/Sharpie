using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpie
{
    public class ClassWriter : BaseWriter
    {
        private readonly HashSet<string> _baseClasses = new HashSet<string>();

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

        public virtual async Task AddField(Accessibility accessibility, bool readOnly, string type, string name)
        {
            await WriteLine(accessibility.ToSharpieString() + (readOnly ? " readonly " : " ") + type + " " + name + ";").ConfigureAwait(false);
            await WriteLine().ConfigureAwait(false);
        }

        public virtual async Task AddField(Accessibility accessibility, string type, string name) => await AddField(accessibility, false, type, name).ConfigureAwait(false);

        public virtual async Task AddField<T>(Accessibility accessibility, bool readOnly, string name) => await AddField(accessibility, readOnly, GetCSharpTypeName(typeof(T).Name), name).ConfigureAwait(false);

        public virtual async Task AddField<T>(Accessibility accessibility, string name) => await AddField(accessibility, GetCSharpTypeName(typeof(T).Name), name).ConfigureAwait(false);
    }
}
