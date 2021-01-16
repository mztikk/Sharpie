using System;
using System.Threading.Tasks;

namespace Sharpie.Writer.Loops.For
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
            var forLoopHeadWriter = new ForLoopHeadWriter(writer, forLoop);
            forLoopHeadWriter.Make();

            writer.OpenBrackets();

            var forLoopBodyWriter = new ForLoopBodyWriter(writer, forLoop);
            forLoopBodyWriter.Make();

            writer.CloseBrackets();
        }
    }
}
