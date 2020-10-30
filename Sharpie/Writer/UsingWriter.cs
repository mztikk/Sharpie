using System;
using System.Collections.Generic;

namespace Sharpie.Writer
{
    public class UsingWriter : BaseWriter
    {
        private readonly HashSet<string> _usings = new HashSet<string>();

        public UsingWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public override bool DidWork { get; protected set; }

        public void AddUsing(string name) => _usings.Add(name);

        private IEnumerable<string> GetUsingStatements()
        {
            foreach (string item in _usings)
            {
                yield return "using " + item + ";";
            }
        }

        public string GetUsing() => string.Join(Environment.NewLine, GetUsingStatements());

        protected override void Finish()
        {
            if (_usings.Count > 0)
            {
                WriteLine(GetUsing());
                DidWork = true;
            }
        }

        protected override void Start() { }
    }
}
