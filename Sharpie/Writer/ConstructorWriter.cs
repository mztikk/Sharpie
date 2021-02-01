using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Sharpie.Writer
{
    public class ConstructorWriter : BaseWriter
    {
        private readonly List<Constructor> _ctors = new List<Constructor>();
        private readonly string _name;

        public ConstructorWriter(IndentedStreamWriter writer, string name) : base(writer) => _name = name;
        public ConstructorWriter(IndentedStreamWriter writer, string name, IEnumerable<Constructor> ctors) : this(writer, name) => _ctors.AddRange(ctors);

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Constructor ctor) => _ctors.Add(ctor);

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<Parameter> arguments, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, arguments, body));

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility = Accessibility.Public) => AddConstructor(accessibility, Array.Empty<Parameter>(), string.Empty);

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<string> BaseCtorParameters, IEnumerable<Parameter> arguments, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, BaseCtorParameters, arguments, body));
        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<Parameter> arguments, IEnumerable<string> ThisCtorParameters, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, arguments, ThisCtorParameters, body));

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<Parameter> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, body));

        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<string> BaseCtorParameters, IEnumerable<Parameter> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, BaseCtorParameters, arguments, body));
        [Obsolete("Use Class API", true)]
        public void AddConstructor(Accessibility accessibility, IEnumerable<Parameter> arguments, IEnumerable<string> ThisCtorParameters, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, ThisCtorParameters, body));

        protected override bool Start() => false;

        protected override bool Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _ctors.Count; i++)
            {
                // new line between ctors (before everyone except the first one)
                if (i > 0)
                {
                    WriteLine();
                }

                sb.Append(_ctors[i].Accessibility.ToSharpieString());
                sb.Append(" ");
                sb.Append(_ctors[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", _ctors[i].Parameters));
                sb.Append(")");
                if (_ctors[i].BaseCtorParameters is { })
                {
                    sb.Append(" : base(");
                    sb.Append(string.Join(", ", _ctors[i].BaseCtorParameters));
                    sb.Append(")");
                }
                else if (_ctors[i].ThisCtorParameters is { })
                {
                    sb.Append(" : this(");
                    sb.Append(string.Join(", ", _ctors[i].ThisCtorParameters));
                    sb.Append(")");
                }
                WriteLine(sb.ToString());
                sb.Clear();

                OpenBrackets();

                _ctors[i].Body(_writer);

                CloseBrackets();

                sb.Clear();
            }

            return _ctors.Count > 0;
        }
    }
}
