using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class NamespaceWriter : BaseWriter
    {
        public NamespaceWriter(IndentedStreamWriter writer, string? nameSpace) : base(writer) => Namespace = nameSpace;

        public override bool DidWork { get; protected set; }
        public string? Namespace { get; }

        protected override async Task Start()
        {
            if (Namespace is { })
            {
                await WriteLineAsync("namespace " + Namespace).ConfigureAwait(false);
                await WriteLineAsync("{").ConfigureAwait(false);
                IndentationLevel++;
            }
        }

        protected override async Task Finish()
        {
            if (Namespace is { })
            {
                IndentationLevel--;
                await WriteLineAsync("}").ConfigureAwait(false);
                DidWork = true;
            }
        }
    }
}
