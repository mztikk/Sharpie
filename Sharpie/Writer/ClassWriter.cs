using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public static class ClassWriter
    {
        private static string GetInheritance(this Class c)
        {
            if (c._baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", c._baseClasses);
        }

        public static async Task Write(Class c, Stream stream)
        {
            var writer = new IndentedStreamWriter(stream);

            var usingWriter = new UsingWriter(writer);
            var namespaceWriter = new NamespaceWriter(writer, c.Namespace);

            BaseWriter[] bodyWriters = new BaseWriter[]
            {
                new FieldWriter(writer, c._fields),
                new ConstructorWriter(writer, c.ClassName, c._ctors),
                new PropertyWriter(writer, c._properties),
                new MethodWriter(writer, c._methods)
            };

            foreach (string? u in c._usings)
            {
                usingWriter.AddUsing(u);
            }

            await usingWriter.Make().ConfigureAwait(false);
            if (usingWriter.DidWork)
            {
                await writer.WriteLineAsync().ConfigureAwait(false);
            }

            var classDescription = new List<string>();
            if (c.Accessibility.HasValue)
            {
                classDescription.Add(c.Accessibility.Value.ToSharpieString());
            }
            if (c.Static)
            {
                classDescription.Add("static");
            }

            classDescription.Add("class");
            classDescription.Add(c.ClassName);

            await namespaceWriter.Begin().ConfigureAwait(false);
            await writer.WriteLineAsync(string.Join(" ", classDescription) + c.GetInheritance()).ConfigureAwait(false);
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            BaseWriter? prevWriter = null;
            foreach (BaseWriter bodyWriter in bodyWriters)
            {
                if (prevWriter is { } && prevWriter.DidWork)
                {
                    await writer.WriteLineAsync().ConfigureAwait(false);
                }

                await bodyWriter.Make().ConfigureAwait(false);

                prevWriter = bodyWriter;
            }

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
            await namespaceWriter.End().ConfigureAwait(false);
        }

        public static async Task<string> Write(Class c)
        {
            using (var stream = new MemoryStream())
            {
                await Write(c, stream).ConfigureAwait(false);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
