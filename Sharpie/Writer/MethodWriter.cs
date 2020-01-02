using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class MethodWriter : BaseWriter
    {
        private readonly List<Method> _methods = new List<Method>();

        public MethodWriter(IndentedStreamWriter writer) : base(writer) { }
        public override bool DidWork { get; protected set; }

        public void AddMethod(Method method) => _methods.Add(method);

        public void AddMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => AddMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        public void AddMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => AddMethod(accessibility, false, false, returnType, name, arguments, body);

        public void AddMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => AddMethod(accessibility, false, async, returnType, name, arguments, body);

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _methods.Count; i++)
            {
                // new line between methods (before everyone except the first one)
                if (i > 0)
                {
                    await WriteLineAsync().ConfigureAwait(false);
                }

                sb.Append(_methods[i].Accessibility.ToSharpieString());
                if (_methods[i].Static)
                {
                    sb.Append(" static");
                }
                if (_methods[i].Async)
                {
                    sb.Append(" async");
                }
                sb.Append(" ");
                sb.Append(_methods[i].ReturnType);
                sb.Append(" ");
                sb.Append(_methods[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", _methods[i].Arguments));
                sb.Append(")");
                await WriteLineAsync(sb.ToString()).ConfigureAwait(false);
                sb.Clear();
                await WriteLineAsync("{").ConfigureAwait(false);

                IndentationLevel++;
                //foreach (string line in _methods[i].Body.GetLines())
                //{
                //    await WriteLine(line).ConfigureAwait(false);
                //}
                _methods[i].Body(_writer);
                IndentationLevel--;

                await WriteLineAsync("}").ConfigureAwait(false);

                DidWork = true;
            }
        }
    }
}
