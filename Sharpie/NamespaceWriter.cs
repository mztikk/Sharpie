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

        public override async Task Begin()
        {
            await WriteLine("namespace " + Namespace).ConfigureAwait(false);
            await WriteLine("{").ConfigureAwait(false);
            IndentationLevel++;
        }

        public override async Task End()
        {
            IndentationLevel--;
            await WriteLine("}").ConfigureAwait(false);
        }
    }
}
