using System;
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

        public static void Write(If @if, IndentedStreamWriter writer)
        {
            writer.WriteLine($"if ({@if.Condition})");
            writer.WriteLine("{");
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            @if.Body(bodyWriter);

            writer.IndentationLevel--;
            writer.WriteLine("}");
        }
    }
}
