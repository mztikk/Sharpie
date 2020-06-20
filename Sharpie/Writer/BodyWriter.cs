using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class BodyWriter
    {
        private readonly IndentedStreamWriter _writer;

        public BodyWriter(IndentedStreamWriter writer) => _writer = writer;

        public virtual async Task<BodyWriter> WriteLineAsync(string s = "")
        {
            await _writer.WriteLineAsync(s).ConfigureAwait(false);
            return this;
        }

        public virtual async Task<BodyWriter> WriteAsync(string s = "")
        {
            await _writer.WriteAsync(s).ConfigureAwait(false);
            return this;
        }

        public virtual BodyWriter WriteLine(string s = "")
        {
            _writer.WriteLine(s);
            return this;
        }

        public virtual BodyWriter Write(string s = "")
        {
            _writer.Write(s);
            return this;
        }

        public async Task<BodyWriter> WriteForLoopAsync(ForLoop forLoop)
        {
            await ForLoopWriter.WriteAsync(forLoop, _writer).ConfigureAwait(false);
            return this;
        }

        public BodyWriter WriteForLoop(ForLoop forLoop)
        {
            ForLoopWriter.Write(forLoop, _writer);
            return this;
        }
    }
}
