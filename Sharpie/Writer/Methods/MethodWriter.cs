using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpie.Writer.Methods
{
    public class MethodWriter : BaseWriter
    {
        private readonly List<Method> _methods = new List<Method>();

        public MethodWriter(IndentedStreamWriter writer) : base(writer) { }
        public MethodWriter(IndentedStreamWriter writer, IEnumerable<Method> methods) : this(writer) => _methods.AddRange(methods);

        [Obsolete("Use Class API", true)]
        public void AddMethod(Method method) => _methods.Add(method);

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> arguments, Action<BodyWriter> body) => AddMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> arguments, Action<BodyWriter> body) => AddMethod(accessibility, false, false, returnType, name, arguments, body);

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> arguments, Action<BodyWriter> body) => AddMethod(accessibility, false, async, returnType, name, arguments, body);

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> arguments, string body) => AddMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> arguments, string body) => AddMethod(accessibility, false, false, returnType, name, arguments, body);

        [Obsolete("Use Class API", true)]
        public void AddMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> arguments, string body) => AddMethod(accessibility, false, async, returnType, name, arguments, body);

        protected override bool Start() => false;

        protected override bool Finish()
        {
            for (int i = 0; i < _methods.Count; i++)
            {
                // new line between methods (before everyone except the first one)
                if (i > 0)
                {
                    WriteLine();
                }

                var attributeWriter = new MethodAttributeWriter(_writer, _methods[i]);
                attributeWriter.Make();

                var methodModifierWriter = new MethodModifierWriter(_writer, _methods[i]);
                methodModifierWriter.Make();

                var parameterWriter = new MethodParameterWriter(_writer, _methods[i]);
                if (parameterWriter.Make())
                {
                    WriteLine();
                }

                OpenBrackets();

                var bodyWriter = new BodyWriter(_writer);
                _methods[i].Body(bodyWriter);

                CloseBrackets();
            }

            return _methods.Count > 0;
        }
    }
}
