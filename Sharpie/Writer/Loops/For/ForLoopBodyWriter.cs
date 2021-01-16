namespace Sharpie.Writer.Loops.For
{
    public class ForLoopBodyWriter : BaseWriter
    {
        private readonly ForLoop _forLoop;

        public ForLoopBodyWriter(IndentedStreamWriter writer, ForLoop forLoop) : base(writer) => _forLoop = forLoop;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            var bodyWriter = new BodyWriter(_writer);
            _forLoop.Body(bodyWriter);

            return true;
        }
    }
}
