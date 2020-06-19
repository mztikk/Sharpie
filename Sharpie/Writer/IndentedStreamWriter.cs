using System;
using System.IO;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class IndentedStreamWriter : IDisposable
    {
        protected readonly Stream _stream;
        private readonly StreamWriter _writer;

        public IndentedStreamWriter(Stream stream)
        {
            _stream = stream;
            _writer = new StreamWriter(stream);
        }

        public int IndentationLevel { get; set; } = 0;

        // Default 4 spaces
        public string Indent { get; set; } = new string(' ', 4);

        public virtual async Task WriteLineAsync(string s = "")
        {
            await WriteAsync(s).ConfigureAwait(false);
            await DirectWriteLineAsync().ConfigureAwait(false);
        }

        public virtual async Task WriteAsync(string s)
        {
            for (int i = 0; i < IndentationLevel; i++)
            {
                await DirectWriteAsync(Indent).ConfigureAwait(false);
            }

            await DirectWriteAsync(s).ConfigureAwait(false);
        }

        protected virtual async Task DirectWriteLineAsync(string s = "")
        {
            await _writer.WriteLineAsync(s).ConfigureAwait(false);
            await _writer.FlushAsync().ConfigureAwait(false);
        }

        protected virtual async Task DirectWriteAsync(string s)
        {
            await _writer.WriteAsync(s).ConfigureAwait(false);
            await _writer.FlushAsync().ConfigureAwait(false);
        }

        public virtual void WriteLine(string s = "")
        {
            Write(s);
            DirectWriteLine();
        }

        public virtual void Write(string s)
        {
            for (int i = 0; i < IndentationLevel; i++)
            {
                DirectWrite(Indent);
            }

            DirectWrite(s);
        }

        protected virtual void DirectWriteLine(string s = "")
        {
            _writer.WriteLine(s);
            _writer.Flush();
        }

        protected virtual void DirectWrite(string s)
        {
            _writer.Write(s);
            _writer.Flush();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _writer?.Dispose();
                    _stream?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~IndentedStreamWriter()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() =>
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
        #endregion

        public static void NopWriter(IndentedStreamWriter writer) { }
    }
}
