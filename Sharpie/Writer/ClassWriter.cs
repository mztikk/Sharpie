using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Sharpie.Writer.Methods;

namespace Sharpie.Writer
{
    public static class ClassWriter
    {
        private static string GetInheritance(this Class c)
        {
            ImmutableArray<string>? baseClasses = c.BaseClasses;

            if (baseClasses?.Length == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", baseClasses);
        }

        [Obsolete("Use sync", true)]
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

            if (usingWriter.Make())
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
            if (c.Partial)
            {
                classDescription.Add("partial");
            }

            classDescription.Add("class");
            classDescription.Add(c.ClassName);

            namespaceWriter.Begin();
            await writer.WriteLineAsync(string.Join(" ", classDescription) + c.GetInheritance());
            await writer.WriteLineAsync("{");
            writer.IndentationLevel++;

            BaseWriter? prevWriter = null;
            bool prevWroteSomething = false;
            foreach (BaseWriter bodyWriter in bodyWriters)
            {
                if (prevWriter is { } && prevWroteSomething)
                {
                    await writer.WriteLineAsync();
                }

                prevWroteSomething = bodyWriter.Make();

                prevWriter = bodyWriter;
            }

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}");
            namespaceWriter.End();
        }

        [Obsolete("Use sync", true)]
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

        public static void Write(Class c, Stream stream)
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

            if (usingWriter.Make())
            {
                writer.WriteLine();
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
            if (c.Partial)
            {
                classDescription.Add("partial");
            }

            classDescription.Add("class");
            classDescription.Add(c.ClassName);

            namespaceWriter.Begin();
            writer.WriteLine(string.Join(" ", classDescription) + c.GetInheritance());
            writer.WriteLine("{");
            writer.IndentationLevel++;

            BaseWriter? prevWriter = null;
            bool prevWroteSomething = false;
            foreach (BaseWriter bodyWriter in bodyWriters)
            {
                if (prevWriter is { } && prevWroteSomething)
                {
                    writer.WriteLine();
                }

                prevWroteSomething = bodyWriter.Make();

                prevWriter = bodyWriter;
            }

            writer.IndentationLevel--;
            writer.WriteLine("}");
            namespaceWriter.End();
        }

        public static string Write(Class c)
        {
            using (var stream = new MemoryStream())
            {
                Write(c, stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
