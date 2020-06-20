namespace Sharpie
{
    public readonly struct Argument
    {
        public Argument(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Type { get; }
        public string Name { get; }

        public override string ToString() => Type + " " + Name;
    }
}
