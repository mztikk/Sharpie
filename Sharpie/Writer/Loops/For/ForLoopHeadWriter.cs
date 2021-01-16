namespace Sharpie.Writer.Loops.For
{
    public class ForLoopHeadWriter : BaseWriter
    {
        private readonly ForLoop _forLoop;

        public ForLoopHeadWriter(IndentedStreamWriter writer, ForLoop forLoop) : base(writer) => _forLoop = forLoop;

        protected override bool Start() => false;
        protected override bool Finish()
        {
            WriteLine($"for ({_forLoop.Initializer}; {_forLoop.Condition}; {_forLoop.Iterator})");

            return true;
        }
    }
}
