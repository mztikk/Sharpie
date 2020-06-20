using System;
using Sharpie.Writer;

namespace Sharpie
{
    public class ForLoop
    {
        public readonly string Initializer;
        public readonly string Condition;
        public readonly string Iterator;
        public readonly Action<BodyWriter> Body;

        public ForLoop(string initializer, string condition, string iterator, Action<BodyWriter> body)
        {
            Initializer = initializer;
            Condition = condition;
            Iterator = iterator;
            Body = body;
        }

        public ForLoop(string initializer, string condition, string iterator, string body) : this(initializer, condition, iterator, StringHelper.StringToBodyWriter(body)) { }

        public ForLoop(string bound, Action<BodyWriter> body) : this("long i = 0", $"i < {bound}", "i++", body) { }
        public ForLoop(string bound, string body) : this(bound, StringHelper.StringToBodyWriter(body)) { }
        public ForLoop(long startValue, string bound, Action<BodyWriter> body) : this($"long i = {startValue}", $"i < {bound}", "i++", body) { }
        public ForLoop(long startValue, string bound, string body) : this(startValue, bound, StringHelper.StringToBodyWriter(body)) { }
    }
}
