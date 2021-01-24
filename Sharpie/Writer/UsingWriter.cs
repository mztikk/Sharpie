using System;
using System.Collections.Generic;

namespace Sharpie.Writer
{
    public class UsingWriter : BaseWriter
    {
        private readonly HashSet<string> _usings;

        public UsingWriter(IndentedStreamWriter writer, IEnumerable<string> usings) : base(writer) => _usings = new HashSet<string>(usings);

        [Obsolete]
        public UsingWriter(IndentedStreamWriter writer) : base(writer) => _usings = new HashSet<string>();

        [Obsolete]
        public void AddUsing(string name) => _usings.Add(name);

        private IEnumerable<string> GetUsingStatements()
        {
            foreach (string item in _usings)
            {
                yield return "using " + item + ";";
            }
        }

        public string GetUsing() => string.Join(Environment.NewLine, GetUsingStatements());

        protected override bool Finish()
        {
            if (_usings.Count > 0)
            {
                WriteLine(GetUsing());
            }

            return _usings.Count > 0;
        }

        protected override bool Start() => false;
    }
}
