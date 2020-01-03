using System;
using Sharpie.Writer;

namespace Sharpie
{
    public class Property
    {
        public readonly Accessibility Accessibility;
        public readonly string Type;
        public readonly string Name;
        public readonly Accessibility? GetterAccessibility;
        public readonly Action<IndentedStreamWriter>? GetterBody;
        public readonly Accessibility? SetterAccessibility;
        public readonly Action<IndentedStreamWriter>? SetterBody;
        public readonly string? InitialValue;

        public Property(
            Accessibility accessibility,
            string type,
            string name,
            Accessibility? getterAccessibility,
            Action<IndentedStreamWriter>? getterBodyAction,
            Accessibility? setterAccessibility,
            Action<IndentedStreamWriter>? setterBodyAction,
            string? initialValue)
        {
            Accessibility = accessibility;
            Type = type;
            Name = name;
            GetterAccessibility = getterAccessibility;
            GetterBody = getterBodyAction;
            SetterAccessibility = setterAccessibility;
            SetterBody = setterBodyAction;
            InitialValue = initialValue;
        }

        public Property(Accessibility accessibility, string type, string name)
            : this(accessibility, type, name, null, getterBodyAction: null, null, null, null) { }

        public Property(Accessibility accessibility, string type, string name, Accessibility getterAccessibility, Accessibility setterAccessibility)
            : this(accessibility, type, name, getterAccessibility, getterBodyAction: null, setterAccessibility, null, null) { }

        public Property(Accessibility accessibility, string type, string name, Action<IndentedStreamWriter> getterBody, Action<IndentedStreamWriter> setterBody)
            : this(accessibility, type, name, null, getterBody, null, setterBody, null) { }

        public Property(Accessibility accessibility, string type, string name, string getterBody, string setterBody)
            : this(accessibility, type, name, null, StringHelper.StringToCall(getterBody), null, StringHelper.StringToCall(setterBody), null) { }

        public Property(
            Accessibility accessibility,
            string type,
            string name,
            Accessibility? getterAccessibility,
            string? getterBody,
            Accessibility? setterAccessibility,
            string? setterBody,
            string? initialValue)
            : this(
                accessibility,
                type,
                name,
                getterAccessibility,
                getterBody is { } ? StringHelper.StringToCall(getterBody) : null,
                setterAccessibility,
                setterBody is { } ? StringHelper.StringToCall(setterBody) : null,
                initialValue)
        { }
    }
}
