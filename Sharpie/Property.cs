namespace Sharpie
{
    public class Property
    {
        public readonly Accessibility Accessibility;
        public readonly string Type;
        public readonly string Name;
        public readonly Accessibility? GetterAccessibility;
        public readonly string? GetterBody;
        public readonly Accessibility? SetterAccessibility;
        public readonly string? SetterBody;
        public readonly string? InitialValue;

        public Property(
            Accessibility accessibility,
            string type,
            string name,
            Accessibility? getterAccessibility,
            string? getterBody,
            Accessibility? setterAccessibility,
            string? setterBody,
            string? initialValue)
        {
            Accessibility = accessibility;
            Type = type;
            Name = name;
            GetterAccessibility = getterAccessibility;
            GetterBody = getterBody;
            SetterAccessibility = setterAccessibility;
            SetterBody = setterBody;
            InitialValue = initialValue;
        }

        public Property(Accessibility accessibility, string type, string name)
            : this(accessibility, type, name, null, null, null, null, null) { }

        public Property(Accessibility accessibility, string type, string name, Accessibility getterAccessibility, Accessibility setterAccessibility)
            : this(accessibility, type, name, getterAccessibility, null, setterAccessibility, null, null) { }
    }
}
