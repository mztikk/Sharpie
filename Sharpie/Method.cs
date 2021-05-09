using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct Method
    {
        public readonly Accessibility Accessibility;
        public readonly bool Static;
        public readonly bool Async;
        public readonly string ReturnType;
        public readonly string Name;
        public readonly ImmutableArray<Parameter> Parameters;
        public readonly ImmutableArray<Attribute> Attributes;
        public readonly Action<BodyWriter> Body;

        public Method(Accessibility accessibility,
                      bool Static,
                      bool async,
                      string returnType,
                      string name,
                      IEnumerable<Parameter> parameters,
                      IEnumerable<Attribute> attributes,
                      Action<BodyWriter> body)
        {
            Accessibility = accessibility;
            this.Static = Static;
            Async = async;
            ReturnType = returnType;
            Name = name;
            Parameters = parameters.ToImmutableArray();
            Attributes = attributes.ToImmutableArray();
            Body = body;
        }

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body)
            : this(accessibility, Static, async, returnType, name, parameters, Array.Empty<Attribute>(), body) { }

        public Method(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body)
            : this(accessibility, false, false, returnType, name, parameters, body) { }

        public Method(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body)
            : this(accessibility, false, async, returnType, name, parameters, body) { }

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> parameters, string body)
            : this(accessibility, Static, async, returnType, name, parameters, StringHelper.StringToBodyWriter(body)) { }

        public Method(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> parameters, string body)
            : this(accessibility, false, false, returnType, name, parameters, body) { }

        public Method(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> parameters, string body)
            : this(accessibility, false, async, returnType, name, parameters, body) { }

        public Method WithAttribute(Attribute attribute)
        {
            var attributes = new List<Attribute>(Attributes)
            {
                attribute
            };

            return new Method(Accessibility, Static, Async, ReturnType, Name, Parameters, attributes, Body);
        }

        public Method Inline() => WithAttribute(Attribute.InlineAttribute);
    }
}
