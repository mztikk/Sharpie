using System.Threading.Tasks;

namespace Sharpie
{
    public abstract class BaseWriter
    {
        protected readonly IndentedStreamWriter _writer;

        protected BaseWriter(IndentedStreamWriter writer) => _writer = writer;

        public abstract Task Begin();

        public abstract Task End();

        public virtual async Task Run()
        {
            await Begin().ConfigureAwait(false);
            await End().ConfigureAwait(false);
        }

        public async Task WriteLine(string s = "") => await _writer.WriteLine(s).ConfigureAwait(false);

        public int IndentationLevel
        {
            get => _writer.IndentationLevel;
            set => _writer.IndentationLevel = value;
        }

        public string Indent
        {
            get => _writer.Indent;
            set => _writer.Indent = value;
        }
    }
}
