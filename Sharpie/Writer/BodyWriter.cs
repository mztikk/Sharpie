using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class BodyWriter
    {
        private readonly IndentedStreamWriter _writer;

        public BodyWriter(IndentedStreamWriter writer) => _writer = writer;

        public int IndentationLevel { get => _writer.IndentationLevel; set => _writer.IndentationLevel = value; }

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

        [System.Obsolete("Use Async", true)]
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

        [System.Obsolete("Use Async", true)]
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

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteIf(If @if)
        {
            IfWriter.Write(@if, _writer);
            return this;
        }

        public async Task<BodyWriter> WriteSwitchCaseStatementAsync(SwitchCaseStatement switchCaseStatement)
        {
            await SwitchCaseStatementWriter.WriteAsync(switchCaseStatement, _writer).ConfigureAwait(false);
            return this;
        }

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteSwitchCaseStatement(SwitchCaseStatement switchCaseStatement)
        {
            SwitchCaseStatementWriter.Write(switchCaseStatement, _writer);
            return this;
        }

        public async Task<BodyWriter> WriteBreakAsync()
        {
            await _writer.WriteLineAsync("break;");
            return this;
        }

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteBreak()
        {
            _writer.WriteLine("break;");
            return this;
        }

        public async Task<BodyWriter> WriteReturnAsync(string returnValue)
        {
            await _writer.WriteLineAsync($"return {returnValue};");
            return this;
        }

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteReturn(string returnValue)
        {
            _writer.WriteLine($"return {returnValue};");
            return this;
        }

        public async Task<BodyWriter> WriteNullCheckAsync(string paramName)
        {
            await _writer.WriteLineAsync($"if ({paramName} is null)");
            await _writer.WriteLineAsync("{");
            _writer.IndentationLevel++;
            await _writer.WriteLineAsync($"throw new ArgumentException(nameof({paramName}));");
            _writer.IndentationLevel--;
            await _writer.WriteLineAsync("}");

            return this;
        }

        [System.Obsolete("Use Async", true)]
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

        [System.Obsolete("Use Async", true)]
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

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteVariable(string name, string value) => WriteVariable("var", name, value);
        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteVariable<T>(string name, string? value = null) => WriteVariable(typeof(T).CSharpName(), name, value);

        [System.Obsolete("Use Async", true)]
        public BodyWriter WriteAssignment(string name, string value)
        {
            _writer.WriteLine($"{name} = {value};");

            return this;
        }

        public async Task<BodyWriter> WriteVariableAsync(string type, string name, string? value = null)
        {
            if (value is { })
            {
                await _writer.WriteLineAsync($"{type} {name} = {value};");
            }
            else
            {
                await _writer.WriteLineAsync($"{type} {name};");
            }

            return this;
        }

        public async Task<BodyWriter> WriteVariableAsync(string name, string value) => await WriteVariableAsync("var", name, value);
        public async Task<BodyWriter> WriteVariableAsync<T>(string name, string? value = null) => await WriteVariableAsync(typeof(T).CSharpName(), name, value);

        public async Task<BodyWriter> WriteAssignmentAsync(string name, string value)
        {
            await _writer.WriteLineAsync($"{name} = {value};");

            return this;
        }
    }
}
