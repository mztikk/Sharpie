using System.Threading.Tasks;

namespace Sharpie.Writer
{
    internal static class SwitchCaseStatementWriter
    {
        public static async Task WriteAsync(SwitchCaseStatement switchCaseStatement, IndentedStreamWriter writer)
        {
            await writer.WriteLineAsync($"switch ({switchCaseStatement.Expression})");
            await writer.WriteLineAsync("{");
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);

            foreach (CaseStatement @case in switchCaseStatement.CaseStatements)
            {
                await writer.WriteLineAsync($"case {@case.Case}:");
                writer.IndentationLevel++;
                await @case.Body(bodyWriter);
                writer.IndentationLevel--;
            }

            if (switchCaseStatement.DefaultCaseBody is { })
            {
                await writer.WriteLineAsync("default:");
                writer.IndentationLevel++;
                await switchCaseStatement.DefaultCaseBody(bodyWriter);
                writer.IndentationLevel--;
            }

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}");
        }

        [System.Obsolete("Use Async", true)]
        public static void Write(SwitchCaseStatement switchCaseStatement, IndentedStreamWriter writer)
        {
            writer.WriteLine($"switch ({switchCaseStatement.Expression})");
            writer.WriteLine("{");
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);

            foreach (CaseStatement @case in switchCaseStatement.CaseStatements)
            {
                writer.WriteLine($"case {@case.Case}:");
                writer.IndentationLevel++;
                @case.Body(bodyWriter);
                writer.IndentationLevel--;
            }

            if (switchCaseStatement.DefaultCaseBody is { })
            {
                writer.WriteLine("default:");
                writer.IndentationLevel++;
                switchCaseStatement.DefaultCaseBody(bodyWriter);
                writer.IndentationLevel--;
            }

            writer.IndentationLevel--;
            writer.WriteLine("}");
        }
    }
}
