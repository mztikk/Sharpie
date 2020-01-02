using System.Collections.Generic;
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
        public readonly FieldWriter Fields;
        public readonly ConstructorWriter Ctors;

        public ClassWriter(IndentedStreamWriter writer, string className) : base(writer)
        {
            Usings = new UsingWriter(writer);
            Namespace = new NamespaceWriter(writer)
            {
                Namespace = className
            };
            Fields = new FieldWriter(writer);
            Ctors = new ConstructorWriter(writer, className);

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

        public override async Task Begin()
        {
            await Usings.Begin().ConfigureAwait(false);
            await Usings.End().ConfigureAwait(false);

            await Namespace.Begin().ConfigureAwait(false);
            await WriteLine(Accessibility.ToSharpieString() + " class " + ClassName + GetInheritance()).ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        public override async Task End()
        {
            await Fields.Begin().ConfigureAwait(false);
            await Fields.End().ConfigureAwait(false);

            await Ctors.Begin().ConfigureAwait(false);
            await Ctors.End().ConfigureAwait(false);

            IndentationLevel--;
            await WriteLine("}").ConfigureAwait(false);
            await Namespace.End().ConfigureAwait(false);
        }
    }
}
