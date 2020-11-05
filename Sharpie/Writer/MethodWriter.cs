using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Sharpie.Writer
{
    public class MethodWriter : BaseWriter
    {
        private readonly List<Method> _methods = new List<Method>();

        public MethodWriter(IndentedStreamWriter writer) : base(writer) { }
        public MethodWriter(IndentedStreamWriter writer, IEnumerable<Method> methods) : this(writer) => _methods.AddRange(methods);

        public override bool DidWork { get; protected set; }

        public void AddMethod(Method method) => _methods.Add(method);

        public void AddMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => AddMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        public void AddMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => AddMethod(accessibility, false, false, returnType, name, arguments, body);

        public void AddMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => AddMethod(accessibility, false, async, returnType, name, arguments, body);

        public void AddMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body) => AddMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        public void AddMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, string body) => AddMethod(accessibility, false, false, returnType, name, arguments, body);

        public void AddMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body) => AddMethod(accessibility, false, async, returnType, name, arguments, body);

        protected override void Start() { }

        protected override void Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _methods.Count; i++)
            {
                // new line between methods (before everyone except the first one)
                if (i > 0)
                {
                    WriteLine();
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
                WriteLine(sb.ToString().Trim());
                sb.Clear();
                WriteLine("{");

                IndentationLevel++;

                var bodyWriter = new BodyWriter(_writer);
                _methods[i].Body(bodyWriter);

                IndentationLevel--;

                WriteLine("}");

                DidWork = true;
            }
        }
    }
}
