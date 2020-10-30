using System;
using System.Threading.Tasks;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct If
    {
        public readonly string Condition;
        public readonly Func<BodyWriter, Task> Body;

        public If(string condition, Func<BodyWriter, Task> body)
        {
            Condition = condition;
            Body = body;
        }

        public If(string condition, string body) : this(condition, StringHelper.StringToBodyWriter(body)) { }
    }
}
