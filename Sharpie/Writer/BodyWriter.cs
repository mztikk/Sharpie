﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sharpie.Writer.Loops.For;
using Sharpie.Writer.Loops.ForEach;

namespace Sharpie.Writer
{
    public class BodyWriter
    {
        public static Action<BodyWriter> Empty = (writer) => { };

        private readonly IndentedStreamWriter _writer;

        public BodyWriter(IndentedStreamWriter writer) => _writer = writer;

        [Obsolete("Use sync", true)]
        public virtual async Task<BodyWriter> WriteLineAsync(string s = "")
        {
            await _writer.WriteLineAsync(s).ConfigureAwait(false);
            return this;
        }

        [Obsolete("Use sync", true)]
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

        [Obsolete("Use sync", true)]
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

        [Obsolete("Use sync", true)]
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

        [Obsolete("Use sync", true)]
        public async Task<BodyWriter> WriteIfAsync(If @if)
        {
            await IfWriter.WriteAsync(@if, _writer).ConfigureAwait(false);
            return this;
        }

        public BodyWriter WriteIf(IfStatement ifStatement)
        {
            IfWriter.Write(ifStatement, _writer);
            return this;
        }

        [Obsolete("Use sync", true)]
        public async Task<BodyWriter> WriteSwitchCaseStatementAsync(SwitchCaseStatement switchCaseStatement)
        {
            await SwitchCaseStatementWriter.WriteAsync(switchCaseStatement, _writer).ConfigureAwait(false);
            return this;
        }

        public BodyWriter WriteSwitchCaseStatement(SwitchCaseStatement switchCaseStatement)
        {
            SwitchCaseStatementWriter.Write(switchCaseStatement, _writer);
            return this;
        }

        [Obsolete("Use sync", true)]
        public async Task<BodyWriter> WriteBreakAsync()
        {
            await _writer.WriteLineAsync("break;");
            return this;
        }

        public BodyWriter WriteBreak()
        {
            _writer.WriteLine("break;");
            return this;
        }

        [Obsolete("Use sync", true)]
        public async Task<BodyWriter> WriteReturnAsync(string returnValue)
        {
            await _writer.WriteLineAsync($"return {returnValue};");
            return this;
        }

        public BodyWriter WriteReturn()
        {
            _writer.Write("return ");
            return this;
        }

        public BodyWriter WriteReturn(string returnValue)
        {
            _writer.WriteLine($"return {returnValue};");
            return this;
        }

        public BodyWriter WriteNullCheck(string paramName)
        {
            _writer.WriteLine($"if ({paramName} is null)");
            _writer.OpenBrackets();
            _writer.WriteLine($"throw new ArgumentException(nameof({paramName}));");
            _writer.CloseBrackets();

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

        public BodyWriter WriteSwitchExpression(SwitchCaseExpression switchCaseExpression)
        {
            SwitchCaseExpressionWriter.Write(switchCaseExpression, _writer);
            return this;
        }

        public BodyWriter WriteTernaryConditional(string condition, string consequent, string alternative)
        {
            _writer.Write(condition);
            _writer.Write(" ? ");
            _writer.Write(consequent);
            _writer.Write(" : ");
            _writer.Write(alternative);

            return this;
        }

        public BodyWriter EndStatement()
        {
            _writer.WriteLine(";");

            return this;
        }

        public BodyWriter WriteObjectInitializer(string type, Dictionary<string, string> properties)
        {
            _writer.WriteLine($"new {type} {{");
            _writer.IndentationLevel++;
            foreach (KeyValuePair<string, string> property in properties)
            {
                _writer.WriteLine($"{property.Key} = {property.Value},");
            }
            _writer.IndentationLevel--;
            _writer.Write("}");

            return this;
        }

        public BodyWriter WriteReturnSwitchExpression(SwitchCaseExpression switchCaseExpression) => WriteReturn().WriteSwitchExpression(switchCaseExpression);
    }
}
