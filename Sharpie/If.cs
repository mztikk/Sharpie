using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct If
    {
        public readonly string Condition;
        public readonly Action<BodyWriter> Body;
        public readonly ImmutableArray<ElseIf> ElseIfs;
        public readonly Action<BodyWriter>? ElseBody;

        public If(string condition, Action<BodyWriter> body, IEnumerable<ElseIf> elseIfs, Action<BodyWriter>? elseBody)
        {
            Condition = condition;
            Body = body;
            ElseIfs = elseIfs.ToImmutableArray();
            ElseBody = elseBody;
        }

        public If(string condition, string body) : this(condition, StringHelper.StringToBodyWriter(body), Array.Empty<ElseIf>(), null) { }
        public If(string condition, string body, Action<BodyWriter>? elseBody) : this(condition, StringHelper.StringToBodyWriter(body), Array.Empty<ElseIf>(), elseBody) { }
        public If(string condition, string body, IEnumerable<ElseIf> elseIfs) : this(condition, StringHelper.StringToBodyWriter(body), elseIfs, null) { }
    }

    public readonly struct ElseIf
    {
        public readonly string Condition;
        public readonly Action<BodyWriter> Body;

        public ElseIf(string condition, Action<BodyWriter> body)
        {
            Condition = condition;
            Body = body;
        }

        public ElseIf(string condition, string body) : this(condition, StringHelper.StringToBodyWriter(body)) { }
    }
}
