using System;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public abstract class BaseWriter
    {
        protected readonly IndentedStreamWriter _writer;

        protected BaseWriter(IndentedStreamWriter writer) => _writer = writer;

        /// <summary>
        /// Returns <see langword="true"/> if it wrote anything; <see langword="false"/> otherwise
        /// </summary>
        /// <returns></returns>
        public bool Begin() => Start();

        /// <summary>
        /// Returns <see langword="true"/> if it wrote anything; <see langword="false"/> otherwise
        /// </summary>
        /// <returns></returns>
        public bool End() => Finish();

        /// <summary>
        /// Returns <see langword="true"/> if it wrote anything; <see langword="false"/> otherwise
        /// </summary>
        /// <returns></returns>
        public bool Make()
        {
            bool begin = Begin();
            bool end = End();

            return begin || end;
        }

        /// <summary>
        /// Should return <see langword="true"/> if it wrote anything; <see langword="false"/> otherwise
        /// </summary>
        /// <returns></returns>
        protected abstract bool Start();

        /// <summary>
        /// Should return <see langword="true"/> if it wrote anything; <see langword="false"/> otherwise
        /// </summary>
        /// <returns></returns>
        protected abstract bool Finish();

        [Obsolete("Use sync", true)]
        public async Task WriteLineAsync(string s = "") => await _writer.WriteLineAsync(s);

        protected void WriteLine(string s = "") => _writer.WriteLine(s);

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
