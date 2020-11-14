using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sharpie
{
    public readonly struct SwitchCaseExpression
    {
        public readonly string Expression;
        public readonly ImmutableArray<CaseExpression> CaseExpressions;
        public readonly string? DefaultCaseBody;

        public SwitchCaseExpression(string expression, IEnumerable<CaseExpression> caseExpressions, string? defaultCaseBodyAction)
        {
            Expression = expression;
            CaseExpressions = caseExpressions.ToImmutableArray();
            DefaultCaseBody = defaultCaseBodyAction;
        }

        public SwitchCaseExpression(string expression, IEnumerable<CaseExpression> caseExpressions) : this(expression, caseExpressions, defaultCaseBodyAction: null) { }
    }

    public readonly struct CaseExpression
    {
        public readonly string Case;
        public readonly string Body;

        public CaseExpression(string @case, string body)
        {
            Case = @case;
            Body = body;
        }
    }
}
