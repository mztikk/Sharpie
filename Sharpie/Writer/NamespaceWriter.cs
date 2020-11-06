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
                WriteLine("{");
                IndentationLevel++;

                return true;
            }

            return false;
        }

        protected override bool Finish()
        {
            if (Namespace is { })
            {
                IndentationLevel--;
                WriteLine("}");

                return true;
            }

            return false;
        }
    }
}
