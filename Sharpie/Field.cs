namespace Sharpie
{
    public class Field
    {
        public Field(Accessibility accessibility, bool readOnly, string type, string name, string? initialValue)
        {
            Accessibility = accessibility;
            ReadOnly = readOnly;
            Type = type;
            Name = name;
            InitialValue = initialValue;
        }

        public Accessibility Accessibility { get; }
        public bool ReadOnly { get; }
        public string Type { get; }
        public string Name { get; }
        public string? InitialValue { get; }

        public override string ToString() => Accessibility.ToSharpieString() + (ReadOnly ? " readonly " : " ") + Type + " " + Name + (InitialValue is { } ? " = " + InitialValue : "") + ";";
    }
}
