using Microsoft.CodeAnalysis;

namespace Sharpie
{
    public readonly struct Field
    {
        public readonly Accessibility Accessibility;
        public readonly bool ReadOnly;
        public readonly bool IsStatic;
        public readonly bool IsConst;
        public readonly string Type;
        public readonly string Name;
        public readonly string? InitialValue;

        public Field(Accessibility accessibility, bool readOnly, bool isStatic, string type, string name, string? initialValue = null)
        {
            Accessibility = accessibility;
            ReadOnly = readOnly;
            IsStatic = isStatic;
            IsConst = false;
            Type = type;
            Name = name;
            InitialValue = initialValue;
        }

        public Field(Accessibility accessibility, bool isConst, string type, string name, string? initialValue = null)
        {
            Accessibility = accessibility;
            ReadOnly = false;
            IsStatic = false;
            IsConst = isConst;
            Type = type;
            Name = name;
            InitialValue = initialValue;
        }

        public Field(Accessibility accessibility, string type, string name)
            : this(accessibility, false, false, type, name, null) { }
    }
}
