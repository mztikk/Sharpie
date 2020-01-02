using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpie
{
    public class ConstructorWriter : BaseWriter
    {
        private readonly List<Constructor> _ctors = new List<Constructor>();
        private readonly string _name;

        public ConstructorWriter(IndentedStreamWriter writer, string name) : base(writer) => _name = name;

        public void AddConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, string body) => _ctors.Add(new Constructor(accessibility, _name, arguments, body));

        public void AddConstructor(Accessibility accessibility = Accessibility.Public) => AddConstructor(accessibility, Array.Empty<Argument>(), string.Empty);

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            foreach (Constructor ctor in _ctors)
            {
                await WriteLine(ctor.Accessibility.ToSharpieString() + " " + ctor.Name + "(" + string.Join(", ", ctor.Arguments) + ")").ConfigureAwait(false);
                await WriteLine("{").ConfigureAwait(false);

                IndentationLevel++;
                foreach (string line in ctor.Body.GetLines())
                {
                    await WriteLine(line).ConfigureAwait(false);
                }
                IndentationLevel--;

                await WriteLine("}").ConfigureAwait(false);
            }
        }
    }
}
