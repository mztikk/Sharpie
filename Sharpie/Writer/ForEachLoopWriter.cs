using System.Threading.Tasks;

namespace Sharpie.Writer
{
    internal static class ForEachLoopWriter
    {
        public static async Task WriteAsync(ForEachLoop forEachLoop, IndentedStreamWriter writer)
        {
            await writer.WriteLineAsync($"foreach ({forEachLoop.Item} in {forEachLoop.Collection})");
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            await forEachLoop.Body(bodyWriter);

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
        }

        [System.Obsolete("Use Async", true)]
        public static void Write(ForEachLoop forEachLoop, IndentedStreamWriter writer)
        {
            writer.WriteLine($"foreach ({forEachLoop.Item} in {forEachLoop.Collection})");
            writer.WriteLine("{");
            writer.IndentationLevel++;

            var bodyWriter = new BodyWriter(writer);
            forEachLoop.Body(bodyWriter);

            writer.IndentationLevel--;
            writer.WriteLine("}");
        }
    }
}
