using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class BodyWriter
    {
        private readonly IndentedStreamWriter _writer;

        public BodyWriter(IndentedStreamWriter writer) => _writer = writer;

        public virtual async Task WriteLineAsync(string s = "") => await _writer.WriteLineAsync(s).ConfigureAwait(false);

        public virtual async Task WriteAsync(string s = "") => await _writer.WriteAsync(s).ConfigureAwait(false);

        public virtual void WriteLine(string s = "") => _writer.WriteLine(s);

        public virtual void Write(string s = "") => _writer.Write(s);

        public async Task WriteForLoopAsync(ForLoop forLoop) => await ForLoopWriter.WriteAsync(forLoop, _writer).ConfigureAwait(false);
        public void WriteForLoop(ForLoop forLoop) => ForLoopWriter.Write(forLoop, _writer);
    }
}
