using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct IfStatement
    {
        public readonly ImmutableArray<If> Ifs;
        public readonly Action<BodyWriter>? ElseBody;

        public IfStatement(IEnumerable<If> ifs, Action<BodyWriter>? elseBodyWriter)
        {
            Ifs = ifs.ToImmutableArray();
            ElseBody = elseBodyWriter;
        }

        public IfStatement(params If[] ifs) : this(ifs, elseBodyWriter: null) { }
        public IfStatement(IEnumerable<If> ifs, string elseBody) : this(ifs, StringHelper.StringToBodyWriter(elseBody)) { }
    }

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
