namespace Sharpie.Writer.Loops.ForEach
{
    public class ForEachLoopBodyWriter : BaseWriter
    {
        private readonly ForEachLoop _forEachLoop;

        public ForEachLoopBodyWriter(IndentedStreamWriter writer, ForEachLoop forEachLoop) : base(writer) => _forEachLoop = forEachLoop;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            var bodyWriter = new BodyWriter(_writer);
            _forEachLoop.Body(bodyWriter);

            return true;
        }
    }
}
