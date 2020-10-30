using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct Property
    {
        public readonly Accessibility Accessibility;
        public readonly string Type;
        public readonly string Name;
        public readonly Accessibility? GetterAccessibility;
        public readonly Func<BodyWriter, Task>? GetterBody;
        public readonly Accessibility? SetterAccessibility;
        public readonly Func<BodyWriter, Task>? SetterBody;
        public readonly string? InitialValue;

        public Property(
            Accessibility accessibility,
            string type,
            string name,
            Accessibility? getterAccessibility,
            Func<BodyWriter, Task>? getterBodyAction,
            Accessibility? setterAccessibility,
            Func<BodyWriter, Task>? setterBodyAction,
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

        public Property(Accessibility accessibility, string type, string name, Func<BodyWriter, Task> getterBody, Func<BodyWriter, Task> setterBody)
            : this(accessibility, type, name, null, getterBody, null, setterBody, null) { }

        public Property(Accessibility accessibility, string type, string name, string getterBody, string setterBody)
            : this(accessibility, type, name, null, StringHelper.StringToBodyWriter(getterBody), null, StringHelper.StringToBodyWriter(setterBody), null) { }

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
                getterBody is { } ? StringHelper.StringToBodyWriter(getterBody) : null,
                setterAccessibility,
                setterBody is { } ? StringHelper.StringToBodyWriter(setterBody) : null,
                initialValue)
        { }
    }
}
