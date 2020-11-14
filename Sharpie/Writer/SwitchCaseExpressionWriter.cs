namespace Sharpie.Writer
{
    internal static class SwitchCaseExpressionWriter
    {
        public static void Write(SwitchCaseExpression switchCaseExpression, IndentedStreamWriter writer)
        {
            writer.WriteLine($"({switchCaseExpression.Expression}) switch");
            writer.OpenBrackets();

            foreach (CaseExpression @case in switchCaseExpression.CaseExpressions)
            {
                writer.WriteLine($"{@case.Case} => {@case.Body},");
            }

            if (switchCaseExpression.DefaultCaseBody is { })
            {
                writer.WriteLine($"_ => {switchCaseExpression.DefaultCaseBody},");
            }

            writer.IndentationLevel--;
            writer.WriteLine("};");
        }
    }
}
