namespace Sharpie.Writer.Methods
{
    public class MethodAttributeWriter : BaseWriter
    {
        private readonly Method _method;

        public MethodAttributeWriter(IndentedStreamWriter writer, Method method) : base(writer) => _method = method;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            bool didWork = false;

            foreach (Attribute attribute in _method.Attributes)
            {
                if (attribute.Parameters.Length > 0)
                {
                    WriteLine($"[{attribute.Name}({string.Join(",", attribute.Parameters)})]");
                }
                else
                {
                    WriteLine($"[{attribute.Name}]");
                }

                didWork = true;
            }

            return didWork;
        }
    }
}
