﻿namespace Sharpie
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

        public readonly Accessibility Accessibility;
        public readonly bool ReadOnly;
        public readonly string Type;
        public readonly string Name;
        public readonly string? InitialValue;
    }
}
