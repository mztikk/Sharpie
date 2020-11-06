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

        private bool _indented = false;

        [Obsolete("Use sync", true)]
        public virtual async Task WriteLineAsync(string s = "")
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                await WriteAsync(s);
            }

            await DirectWriteLineAsync();
        }

        [Obsolete("Use sync", true)]
        public virtual async Task WriteAsync(string s)
        {
            await WriteIndentAsync();

            await DirectWriteAsync(s);
        }

        [Obsolete("Use sync", true)]
        protected virtual async Task DirectWriteLineAsync(string s = "")
        {
            await _writer.WriteLineAsync(s);
            await _writer.FlushAsync();

            _indented = false;
        }

        [Obsolete("Use sync", true)]
        protected virtual async Task DirectWriteAsync(string s)
        {
            await _writer.WriteAsync(s);
            await _writer.FlushAsync();
        }

        public virtual void WriteLine(string s = "")
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                Write(s);
            }

            DirectWriteLine();
        }

        public virtual void Write(string s)
        {
            WriteIndent();

            DirectWrite(s);
        }

        public virtual void OpenBrackets()
        {
            WriteLine("{");
            IndentationLevel++;
        }

        public virtual void CloseBrackets()
        {
            IndentationLevel--;
            WriteLine("}");
        }

        protected virtual void WriteIndent()
        {
            if (_indented)
            {
                return;
            }

            for (int i = 0; i < IndentationLevel; i++)
            {
                DirectWrite(Indent);
            }

            _indented = true;
        }

        [Obsolete("Use sync", true)]
        protected virtual async Task WriteIndentAsync()
        {
            if (_indented)
            {
                return;
            }

            for (int i = 0; i < IndentationLevel; i++)
            {
                await DirectWriteAsync(Indent);
            }

            _indented = true;
        }

        protected virtual void DirectWriteLine(string s = "")
        {
            _writer.WriteLine(s);
            _writer.Flush();

            _indented = false;
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
