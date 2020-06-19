using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class ___ClassWriter : BaseWriter
    {
        private readonly HashSet<string> _baseClasses = new HashSet<string>();
        public readonly string ClassName;
        public readonly UsingWriter Usings;
        protected readonly NamespaceWriter Namespace;
        public readonly FieldWriter Fields;
        public readonly ConstructorWriter Ctors;
        public readonly MethodWriter Methods;
        public readonly PropertyWriter Properties;

        private readonly List<BaseWriter> _bodyWriters = new List<BaseWriter>();

        public ___ClassWriter(IndentedStreamWriter writer, string className, string? nameSpace = null) : base(writer)
        {
            Usings = new UsingWriter(writer);
            Namespace = new NamespaceWriter(writer, nameSpace);
            Fields = new FieldWriter(writer);
            Ctors = new ConstructorWriter(writer, className);
            Properties = new PropertyWriter(writer);
            Methods = new MethodWriter(writer);

            ClassName = className;

            _bodyWriters.Add(Fields);
            _bodyWriters.Add(Ctors);
            _bodyWriters.Add(Properties);
            _bodyWriters.Add(Methods);
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
                await WriteLineAsync().ConfigureAwait(false);
            }

            await Namespace.Begin().ConfigureAwait(false);
            await WriteLineAsync(Accessibility.ToSharpieString() + " class " + ClassName + GetInheritance()).ConfigureAwait(false);
            await WriteLineAsync("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        protected override async Task Finish()
        {
            BaseWriter prevWriter = _bodyWriters[0];
            foreach (BaseWriter writer in _bodyWriters)
            {
                if (prevWriter.DidWork)
                {
                    await WriteLineAsync().ConfigureAwait(false);
                }

                await writer.Begin().ConfigureAwait(false);
                await writer.End().ConfigureAwait(false);

                prevWriter = writer;
            }

            IndentationLevel--;
            await WriteLineAsync("}").ConfigureAwait(false);
            await Namespace.End().ConfigureAwait(false);

            DidWork = true;
        }
    }
}
