namespace Sharpie
{
    public readonly struct Argument
    {
        public Argument(string type, string name, string? defaultValue)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
        }

        public Argument(string type, string name) : this(type, name, null) { }

        public string Type { get; }
        public string Name { get; }
        public string? DefaultValue { get; }

        public override string ToString() => DefaultValue is { } ? $"{Type} {Name} = {DefaultValue}" : Type + " " + Name;
    }
}
