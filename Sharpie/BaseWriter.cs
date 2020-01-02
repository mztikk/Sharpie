using System.Threading.Tasks;

namespace Sharpie
{
    public abstract class BaseWriter
    {
        protected readonly IndentedStreamWriter _writer;

        protected BaseWriter(IndentedStreamWriter writer) => _writer = writer;

        protected bool _started { get; private set; } = false;
        protected bool _finished { get; private set; } = false;

        public async Task Begin()
        {
            if (_started)
            {
                return;
            }

            _started = true;
            await Start().ConfigureAwait(false);
        }

        public async Task End()
        {
            if (!_started || _finished)
            {
                return;
            }

            _finished = true;
            await Finish().ConfigureAwait(false);
        }

        protected abstract Task Start();

        protected abstract Task Finish();

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
