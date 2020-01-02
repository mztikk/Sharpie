using System;
using System.Collections.Generic;
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

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            for (int i = 0; i < _ctors.Count; i++)
            {
                // new line between ctors (before everyone except the first one)
                if (i > 0)
                {
                    await WriteLine().ConfigureAwait(false);
                }

                await WriteLine(_ctors[i].Accessibility.ToSharpieString() + " " + _ctors[i].Name + "(" + string.Join(", ", _ctors[i].Arguments) + ")").ConfigureAwait(false);
                await WriteLine("{").ConfigureAwait(false);

                IndentationLevel++;
                foreach (string line in _ctors[i].Body.GetLines())
                {
                    await WriteLine(line).ConfigureAwait(false);
                }
                IndentationLevel--;

                await WriteLine("}").ConfigureAwait(false);

                DidWork = true;
            }
        }
    }
}
