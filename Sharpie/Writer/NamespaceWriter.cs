namespace Sharpie.Writer
{
    public class NamespaceWriter : BaseWriter
    {
        public NamespaceWriter(IndentedStreamWriter writer, string? nameSpace) : base(writer) => Namespace = nameSpace;

        public override bool DidWork { get; protected set; }
        public string? Namespace { get; }

        protected override void Start()
        {
            if (Namespace is { })
            {
                WriteLine("namespace " + Namespace);
                WriteLine("{");
                IndentationLevel++;
            }
        }

        protected override void Finish()
        {
            if (Namespace is { })
            {
                IndentationLevel--;
                WriteLine("}");
                DidWork = true;
            }
        }
    }
}
