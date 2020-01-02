using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class ConstructorWriter : BaseWriter
    {
        private readonly List<Constructor> _ctors = new List<Constructor>();
        private readonly string _name;

        public ConstructorWriter(IndentedStreamWriter writer, string name) : base(writer) => _name = name;
        public override bool DidWork { get; protected set; }

        public void AddConstructor(Constructor ctor) => _ctors.Add(ctor);

        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, body));

        public void AddConstructor(Accessibility accessibility = Accessibility.Public) => AddConstructor(accessibility, Array.Empty<Argument>(), string.Empty);

        public void AddConstructor(Accessibility accessibility, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, string body) => AddConstructor(new Constructor(accessibility, _name, baseCtorArguments, arguments, body));
        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, string body) => AddConstructor(new Constructor(accessibility, _name, arguments, thisCtorArguments, body));

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _ctors.Count; i++)
            {
                // new line between ctors (before everyone except the first one)
                if (i > 0)
                {
                    await WriteLineAsync().ConfigureAwait(false);
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
                await WriteLineAsync(sb.ToString()).ConfigureAwait(false);
                sb.Clear();
                await WriteLineAsync("{").ConfigureAwait(false);

                IndentationLevel++;
                foreach (string line in _ctors[i].Body.GetLines())
                {
                    await WriteLineAsync(line).ConfigureAwait(false);
                }
                IndentationLevel--;

                await WriteLineAsync("}").ConfigureAwait(false);
                sb.Clear();

                DidWork = true;
            }
        }
    }
}
