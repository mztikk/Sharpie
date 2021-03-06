﻿namespace Sharpie
{
    public readonly struct Parameter
    {
        public Parameter(string type, string name, string? defaultValue, bool thisParameter)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
            ThisParameter = thisParameter;
        }

        public Parameter(string type, string name) : this(type, name, null, false) { }
        public Parameter(string type, string name, bool thisParameter) : this(type, name, null, thisParameter) { }
        public Parameter(string type, string name, string? defaultValue) : this(type, name, defaultValue, false) { }

        public string Type { get; }
        public string Name { get; }
        public string? DefaultValue { get; }
        public bool ThisParameter { get; }

        public override string ToString()
        {
            string parameterString = string.Empty;
            if (ThisParameter)
            {
                parameterString += "this ";
            }
            parameterString += $"{Type} {Name}";
            if (DefaultValue is { })
            {
                parameterString += $" = {DefaultValue}";
            }

            return parameterString;
        }
    }
}
