using System.Collections.Generic;
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
        public override bool DidWork { get; protected set; }

        public void AddBaseClass(string name) => _baseClasses.Add(name);

        private string GetInheritance()
        {
            if (_baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", _baseClasses);
        }

        protected override async Task Start()
        {
            await Usings.Begin().ConfigureAwait(false);
            await Usings.End().ConfigureAwait(false);
            if (Usings.DidWork)
            {
                await WriteLine();
            }

            await Namespace.Begin().ConfigureAwait(false);
            await WriteLine(Accessibility.ToSharpieString() + " class " + ClassName + GetInheritance()).ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        protected override async Task Finish()
        {
            await Fields.Begin().ConfigureAwait(false);
            await Fields.End().ConfigureAwait(false);
            if (Fields.DidWork)
            {
                await WriteLine();
            }

            await Ctors.Begin().ConfigureAwait(false);
            await Ctors.End().ConfigureAwait(false);
            if (Ctors.DidWork)
            {
                await WriteLine();
            }

            IndentationLevel--;
            await WriteLine("}").ConfigureAwait(false);
            await Namespace.End().ConfigureAwait(false);

            DidWork = true;
        }
    }
}
