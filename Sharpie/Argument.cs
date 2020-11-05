namespace Sharpie
{
    public readonly struct Argument
    {
        public Argument(string type, string name, string? defaultValue, bool thisParameter)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
            ThisParameter = thisParameter;
        }

        public Argument(string type, string name) : this(type, name, null, false) { }
        public Argument(string type, string name, bool thisParameter) : this(type, name, null, thisParameter) { }
        public Argument(string type, string name, string? defaultValue) : this(type, name, defaultValue, false) { }

        public string Type { get; }
        public string Name { get; }
        public string? DefaultValue { get; }
        public bool ThisParameter { get; }

        public override string ToString()
        {
            //return DefaultValue is { } ? $"{Type} {Name} = {DefaultValue}" : Type + " " + Name;

            string argumentString = string.Empty;
            if (ThisParameter)
            {
                argumentString += "this ";
            }
            argumentString += $"{Type} {Name}";
            if (DefaultValue is { })
            {
                argumentString += $" = {DefaultValue}";
            }

            return argumentString;
        }
    }
}
