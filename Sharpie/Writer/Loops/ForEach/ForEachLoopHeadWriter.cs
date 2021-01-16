namespace Sharpie.Writer.Loops.ForEach
{
    public class ForEachLoopHeadWriter : BaseWriter
    {
        private readonly ForEachLoop _forEachLoop;

        public ForEachLoopHeadWriter(IndentedStreamWriter writer, ForEachLoop forEachLoop) : base(writer) => _forEachLoop = forEachLoop;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            WriteLine($"foreach ({_forEachLoop.Item} in {_forEachLoop.Collection})");

            return true;
        }
    }
}
