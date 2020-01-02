using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpie
{
    public class UsingWriter : BaseWriter
    {
        private readonly HashSet<string> _usings = new HashSet<string>();

        public UsingWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public void AddUsing(string name) => _usings.Add(name);

        private IEnumerable<string> UsingStatements()
        {
            foreach (string item in _usings)
            {
                yield return "using " + item + ";";
            }
        }

        public string GetUsing()
        {
            if (_usings.Count == 0)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, UsingStatements());
        }

        public override Task Begin() =>
            // NOP
            Task.CompletedTask;

        public override async Task End()
        {
            if (_usings.Count > 0)
            {
                await WriteLine(GetUsing()).ConfigureAwait(false);
                await WriteLine().ConfigureAwait(false);
            }
        }
    }
}
