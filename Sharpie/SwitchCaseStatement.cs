using System;
using System.Collections.Generic;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct SwitchCaseStatement
    {
        public readonly string Expression;
        public readonly IEnumerable<CaseStatement> CaseStatements;
        public readonly Action<BodyWriter>? DefaultCaseBody;

        public SwitchCaseStatement(string expression, IEnumerable<CaseStatement> caseStatements, Action<BodyWriter>? defaultCaseBodyAction)
        {
            Expression = expression;
            CaseStatements = caseStatements;
            DefaultCaseBody = defaultCaseBodyAction;
        }

        public SwitchCaseStatement(string expression, IEnumerable<CaseStatement> caseStatements, string defaultCaseBody) : this(expression, caseStatements, StringHelper.StringToBodyWriter(defaultCaseBody)) { }
        public SwitchCaseStatement(string expression, IEnumerable<CaseStatement> caseStatements) : this(expression, caseStatements, defaultCaseBodyAction: null) { }
    }

    public readonly struct CaseStatement
    {
        public readonly string Case;
        public readonly Action<BodyWriter> Body;

        public CaseStatement(string @case, Action<BodyWriter> body)
        {
            Case = @case;
            Body = body;
        }

        public CaseStatement(string @case, string body) : this(@case, StringHelper.StringToBodyWriter(body)) { }
    }
}
