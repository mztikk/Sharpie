using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public static class ClassWriter
    {
        private static string GetInheritance(this Class c)
        {
            ImmutableList<string>? baseClasses = c.BaseClasses;

            if (baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", baseClasses);
        }

        public static async Task WriteAsync(Class c, Stream stream)
        {
            var writer = new IndentedStreamWriter(stream);

            var usingWriter = new UsingWriter(writer);
            var namespaceWriter = new NamespaceWriter(writer, c.Namespace);

            BaseWriter[] bodyWriters = new BaseWriter[]
            {
                new FieldWriter(writer, c.Fields),
                new ConstructorWriter(writer, c.ClassName, c.Ctors),
                new PropertyWriter(writer, c.Properties),
                new MethodWriter(writer, c.Methods)
            };

            foreach (string? u in c.Usings)
            {
                usingWriter.AddUsing(u);
            }

            await usingWriter.Make();
            if (usingWriter.DidWork)
            {
                await writer.WriteLineAsync();
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

            await namespaceWriter.Begin();
            await writer.WriteLineAsync(string.Join(" ", classDescription) + c.GetInheritance());
            await writer.WriteLineAsync("{");
            writer.IndentationLevel++;

            BaseWriter? prevWriter = null;
            foreach (BaseWriter bodyWriter in bodyWriters)
            {
                if (prevWriter is { } && prevWriter.DidWork)
                {
                    await writer.WriteLineAsync();
                }

                await bodyWriter.Make();

                prevWriter = bodyWriter;
            }

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}");
            await namespaceWriter.End();
        }

        public static async Task<string> WriteAsync(Class c)
        {
            using (var stream = new MemoryStream())
            {
                await WriteAsync(c, stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
