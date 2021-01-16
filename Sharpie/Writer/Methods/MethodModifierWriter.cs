using System.Text;

namespace Sharpie.Writer.Methods
{
    public class MethodModifierWriter : BaseWriter
    {
        private readonly Method _method;

        public MethodModifierWriter(IndentedStreamWriter writer, Method method) : base(writer) => _method = method;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            var sb = new StringBuilder();

            sb.Append(_method.Accessibility.ToSharpieString());
            if (_method.Static)
            {
                sb.Append(" static");
            }
            if (_method.Async)
            {
                sb.Append(" async");
            }
            sb.Append(" ");
            sb.Append(_method.ReturnType);
            sb.Append(" ");
            sb.Append(_method.Name);
            Write(sb.ToString().Trim());

            return true;
        }
    }
}
