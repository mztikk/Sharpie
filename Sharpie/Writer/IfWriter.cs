using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    internal static class IfWriter
    {
        [Obsolete("Use sync", true)]
        public static async Task WriteAsync(If @if, IndentedStreamWriter writer)
        {
            await writer.WriteLineAsync($"if ({@if.Condition})");
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            @if.Body(bodyWriter);

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
        }

        public static void Write(IfStatement ifStatement, IndentedStreamWriter writer)
        {
            if (ifStatement.Ifs.Any())
            {
                If @if = ifStatement.Ifs.First();
                IEnumerable<If> elseIfs = ifStatement.Ifs.Skip(1);

                writer.WriteLine($"if ({@if.Condition})");
                writer.OpenBrackets();
                @if.Body(new BodyWriter(writer));
                writer.CloseBrackets();

                foreach (If elseIf in elseIfs)
                {
                    writer.WriteLine($"if ({elseIf.Condition})");
                    writer.OpenBrackets();
                    elseIf.Body(new BodyWriter(writer));
                    writer.CloseBrackets();
                }
            }
            else
            {
                writer.WriteLine("if (false)");
                writer.OpenBrackets();
                writer.CloseBrackets();
            }

            if (ifStatement.ElseBody is { })
            {
                writer.WriteLine("else");
                writer.OpenBrackets();
                ifStatement.ElseBody(new BodyWriter(writer));
                writer.CloseBrackets();
            }
        }
    }
}
