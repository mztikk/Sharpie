using System;
using System.Threading.Tasks;

namespace Sharpie.Writer.Loops.ForEach
{
    internal static class ForEachLoopWriter
    {
        [Obsolete("Use sync", true)]
        public static async Task WriteAsync(ForEachLoop forEachLoop, IndentedStreamWriter writer)
        {
            await writer.WriteLineAsync($"foreach ({forEachLoop.Item} in {forEachLoop.Collection})");
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            forEachLoop.Body(bodyWriter);

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
        }

        public static void Write(ForEachLoop forEachLoop, IndentedStreamWriter writer)
        {
            var forEachLoopHeadWriter = new ForEachLoopHeadWriter(writer, forEachLoop);
            forEachLoopHeadWriter.Make();

            writer.OpenBrackets();

            var forEachLoopBodyWriter = new ForEachLoopBodyWriter(writer, forEachLoop);
            forEachLoopBodyWriter.Make();

            writer.CloseBrackets();
        }
    }
}
