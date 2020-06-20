using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public abstract class BaseWriter
    {
        protected readonly IndentedStreamWriter _writer;

        protected BaseWriter(IndentedStreamWriter writer) => _writer = writer;

        protected bool _started { get; private set; } = false;
        protected bool _finished { get; private set; } = false;

        public abstract bool DidWork { get; protected set; }

        public async Task Begin()
        {
            if (_started)
            {
                return;
            }

            DidWork = false;
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

        public async Task Make()
        {
            await Begin().ConfigureAwait(false);
            await End().ConfigureAwait(false);
        }

        protected abstract Task Start();

        protected abstract Task Finish();

        public async Task WriteLineAsync(string s = "") => await _writer.WriteLineAsync(s).ConfigureAwait(false);

        public void WriteLine(string s = "") => _writer.WriteLine(s);

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
