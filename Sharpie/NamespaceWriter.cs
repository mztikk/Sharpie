using System.Threading.Tasks;

namespace Sharpie
{
    public class NamespaceWriter : BaseWriter
    {
        public NamespaceWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public override bool DidWork { get; protected set; }
        public string Namespace { get; set; }

        protected override async Task Start()
        {
            await WriteLine("namespace " + Namespace).ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        protected override async Task Finish()
        {
            IndentationLevel--;
            await WriteLine("}").ConfigureAwait(false);
            DidWork = true;
        }
    }
}
