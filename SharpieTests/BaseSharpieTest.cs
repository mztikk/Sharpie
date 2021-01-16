using System;
using System.IO;
using Sharpie.Writer;

namespace SharpieTests
{
    public class BaseSharpieTest
    {
        protected static Stream GetStream() => new MemoryStream();

        protected static IndentedStreamWriter GetIndentedStreamWriter() => new IndentedStreamWriter((GetStream()));

        protected static string GetString(Stream stream)
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream, leaveOpen: true);
            return reader.ReadToEnd();
        }

        protected static string GetString(Action<IndentedStreamWriter> action)
        {
            using Stream stream = GetStream();
            using IndentedStreamWriter indentedWriter = new IndentedStreamWriter(stream);
            action(indentedWriter);
            return GetString(stream);
        }
    }
}
