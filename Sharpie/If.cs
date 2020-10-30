using System;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct If
    {
        public readonly string Condition;
        public readonly Action<BodyWriter> Body;

        public If(string condition, Action<BodyWriter> body)
        {
            Condition = condition;
            Body = body;
        }

        public If(string condition, string body) : this(condition, StringHelper.StringToBodyWriter(body)) { }
    }
}
