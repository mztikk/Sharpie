namespace Sharpie.Writer
{
    public class NamespaceWriter : BaseWriter
    {
        public NamespaceWriter(IndentedStreamWriter writer, string? nameSpace) : base(writer) => Namespace = nameSpace;

        public string? Namespace { get; }

        protected override bool Start()
        {
            if (Namespace is { })
            {
                WriteLine("namespace " + Namespace);
                OpenBrackets();

                return true;
            }

            return false;
        }

        protected override bool Finish()
        {
            if (Namespace is { })
            {
                CloseBrackets();

                return true;
            }

            return false;
        }
    }
}
