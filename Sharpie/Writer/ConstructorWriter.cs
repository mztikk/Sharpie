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

        public void AddConstructor(Constructor ctor) => _ctors.Add(ctor);

        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, arguments, body));

        public void AddConstructor(Accessibility accessibility = Accessibility.Public) => AddConstructor(accessibility, Array.Empty<Argument>(), string.Empty);

        public void AddConstructor(Accessibility accessibility, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, baseCtorArguments, arguments, body));
        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, Action<IndentedStreamWriter> body) => AddConstructor(new Constructor(accessibility, _name, arguments, thisCtorArguments, body));

        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, body));

        public void AddConstructor(Accessibility accessibility, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, baseCtorArguments, arguments, body));
        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, thisCtorArguments, body));

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
                sb.Append(string.Join(", ", _ctors[i].Arguments));
                sb.Append(")");
                if (_ctors[i].BaseCtorArguments is { })
                {
                    sb.Append(" : base(");
                    sb.Append(string.Join(", ", _ctors[i].BaseCtorArguments));
                    sb.Append(")");
                }
                else if (_ctors[i].ThisCtorArguments is { })
                {
                    sb.Append(" : this(");
                    sb.Append(string.Join(", ", _ctors[i].ThisCtorArguments));
                    sb.Append(")");
                }
                WriteLine(sb.ToString());
                sb.Clear();
                WriteLine("{");

                IndentationLevel++;
                //foreach (string line in _ctors[i].Body.GetLines())
                //{
                //    WriteLine(line);
                //}
                _ctors[i].Body(_writer);
                IndentationLevel--;

                WriteLine("}");
                sb.Clear();
            }

            return _ctors.Count > 0;
        }
    }
}
