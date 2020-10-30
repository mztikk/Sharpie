using System;
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

        public void Begin()
        {
            if (_started)
            {
                return;
            }

            DidWork = false;
            _started = true;
            Start();
        }

        public void End()
        {
            if (!_started || _finished)
            {
                return;
            }

            _finished = true;
            Finish();
        }

        public void Make()
        {
            Begin();
            End();
        }

        protected abstract void Start();

        protected abstract void Finish();

        [Obsolete("Use sync", true)]
        public async Task WriteLineAsync(string s = "") => await _writer.WriteLineAsync(s);

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
