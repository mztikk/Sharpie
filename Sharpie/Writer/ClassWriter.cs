using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class ClassWriter
    {
        private readonly Class _class;

        public ClassWriter(Class c) => _class = c;

        private string GetInheritance()
        {
            if (_class._baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", _class._baseClasses);
        }

        public async Task Write(Stream stream)
        {
            var writer = new IndentedStreamWriter(stream);

            var usingWriter = new UsingWriter(writer);
            var namespaceWriter = new NamespaceWriter(writer, _class.Namespace);
            var fieldWriter = new FieldWriter(writer);
            var ctorWriter = new ConstructorWriter(writer, _class.ClassName);
            var propWriter = new PropertyWriter(writer);
            var methodWriter = new MethodWriter(writer);

            List<BaseWriter> bodyWriters = new List<BaseWriter>
            {
                fieldWriter,
                ctorWriter,
                propWriter,
                methodWriter
            };

            foreach (string? u in _class._usings)
            {
                usingWriter.AddUsing(u);
            }

            foreach (Field? field in _class._fields)
            {
                fieldWriter.AddField(field);
            }

            foreach (Constructor? ctor in _class._ctors)
            {
                ctorWriter.AddConstructor(ctor);
            }

            foreach (Property? prop in _class._properties)
            {
                propWriter.AddProperty(prop);
            }

            foreach (Method? method in _class._methods)
            {
                methodWriter.AddMethod(method);
            }

            await usingWriter.Begin().ConfigureAwait(false);
            await usingWriter.End().ConfigureAwait(false);
            if (usingWriter.DidWork)
            {
                await writer.WriteLineAsync().ConfigureAwait(false);
            }

            await namespaceWriter.Begin().ConfigureAwait(false);
            await writer.WriteLineAsync(_class.Accessibility.ToSharpieString() + " class " + _class.ClassName + GetInheritance()).ConfigureAwait(false);
            await writer.WriteLineAsync("{").ConfigureAwait(false);
            writer.IndentationLevel++;

            BaseWriter prevWriter = bodyWriters[0];
            foreach (BaseWriter bodyWriter in bodyWriters)
            {
                if (prevWriter.DidWork)
                {
                    await writer.WriteLineAsync().ConfigureAwait(false);
                }

                await bodyWriter.Begin().ConfigureAwait(false);
                await bodyWriter.End().ConfigureAwait(false);

                prevWriter = bodyWriter;
            }

            writer.IndentationLevel--;
            await writer.WriteLineAsync("}").ConfigureAwait(false);
            await namespaceWriter.End().ConfigureAwait(false);
        }
    }
}
