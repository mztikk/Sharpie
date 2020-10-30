using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct SwitchCaseStatement
    {
        public readonly string Expression;
        public readonly IEnumerable<CaseStatement> CaseStatements;
        public readonly Func<BodyWriter, Task>? DefaultCaseBody;

        public SwitchCaseStatement(string expression, IEnumerable<CaseStatement> caseStatements, Func<BodyWriter, Task>? defaultCaseBodyAction)
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
        public readonly Func<BodyWriter, Task> Body;

        public CaseStatement(string @case, Func<BodyWriter, Task> body)
        {
            Case = @case;
            Body = body;
        }

        public CaseStatement(string @case, string body) : this(@case, StringHelper.StringToBodyWriter(body)) { }
    }
}
