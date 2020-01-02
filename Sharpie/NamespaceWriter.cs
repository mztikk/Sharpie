using System;
using System.IO;
using System.Threading.Tasks;

namespace Sharpie
{
    public class NamespaceWriter : BaseWriter
    {
        public NamespaceWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

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
        }
    }
}
