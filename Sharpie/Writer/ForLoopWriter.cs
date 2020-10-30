using System;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    internal static class ForLoopWriter
    {
        [Obsolete("Use sync", true)]
        public static async Task WriteAsync(ForLoop forLoop, IndentedStreamWriter writer)
        {
            await writer.WriteLineAsync($"for ({forLoop.Initializer}; {forLoop.Condition}; {forLoop.Iterator})").ConfigureAwait(false);
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            forLoop.Body(bodyWriter);

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
        }

        public static void Write(ForLoop forLoop, IndentedStreamWriter writer)
        {
            writer.WriteLine($"for ({forLoop.Initializer}; {forLoop.Condition}; {forLoop.Iterator})");
            writer.WriteLine("{");
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            forLoop.Body(bodyWriter);

            writer.IndentationLevel--;
            writer.WriteLine("}");
        }
    }
}
