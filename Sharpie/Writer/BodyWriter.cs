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

        public async Task<BodyWriter> WriteForEachLoopAsync(ForEachLoop forEachLoop)
        {
            await ForEachLoopWriter.WriteAsync(forEachLoop, _writer).ConfigureAwait(false);
            return this;
        }

        public BodyWriter WriteForEachLoop(ForEachLoop forEachLoop)
        {
            ForEachLoopWriter.Write(forEachLoop, _writer);
            return this;
        }

        public async Task<BodyWriter> WriteIfAsync(If @if)
        {
            await IfWriter.WriteAsync(@if, _writer).ConfigureAwait(false);
            return this;
        }

        public BodyWriter WriteIfLoop(If @if)
        {
            IfWriter.Write(@if, _writer);
            return this;
        }

        public BodyWriter WriteNullCheck(string paramName)
        {
            _writer.WriteLine($"if ({paramName} is null)");
            _writer.WriteLine("{");
            _writer.IndentationLevel++;
            _writer.WriteLine($"throw new ArgumentException(nameof({paramName}));");
            _writer.IndentationLevel--;
            _writer.WriteLine("}");

            return this;
        }

        public int IndentationLevel { get => _writer.IndentationLevel; set => _writer.IndentationLevel = value; }

        public BodyWriter WriteVariable(string type, string name, string? value = null)
        {
            if (value is { })
            {
                _writer.WriteLine($"{type} {name} = {value};");
            }
            else
            {
                _writer.WriteLine($"{type} {name};");
            }

            return this;
        }

        public BodyWriter WriteVariable(string name, string value) => WriteVariable("var", name, value);
        public BodyWriter WriteVariable<T>(string name, string? value = null) => WriteVariable(typeof(T).CSharpName(), name, value);

        public BodyWriter WriteAssignment(string name, string value)
        {
            _writer.WriteLine($"{name} = {value};");

            return this;
        }
    }
}
