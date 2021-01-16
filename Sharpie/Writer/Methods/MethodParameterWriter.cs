namespace Sharpie.Writer.Methods
{
    public class MethodParameterWriter : BaseWriter
    {
        private readonly Method _method;

        public MethodParameterWriter(IndentedStreamWriter writer, Method method) : base(writer) => _method = method;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            // (param1, param2)
            Write($"({string.Join(", ", _method.Parameters)})");

            return true;
        }
    }
}
