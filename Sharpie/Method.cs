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
        public readonly ImmutableArray<Argument> Arguments;
        public readonly ImmutableArray<Attribute> Attributes;
        public readonly Action<BodyWriter> Body;

        public Method(Accessibility accessibility,
                      bool Static,
                      bool async,
                      string returnType,
                      string name,
                      IEnumerable<Argument> arguments,
                      IEnumerable<Attribute> attributes,
                      Action<BodyWriter> body)
        {
            Accessibility = accessibility;
            this.Static = Static;
            Async = async;
            ReturnType = returnType;
            Name = name;
            Arguments = arguments.ToImmutableArray();
            Attributes = attributes.ToImmutableArray();
            Body = body;
        }

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body)
            : this(accessibility, Static, async, returnType, name, arguments, Array.Empty<Attribute>(), body) { }

        public Method(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body)
            : this(accessibility, false, false, returnType, name, arguments, body) { }

        public Method(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body)
            : this(accessibility, false, async, returnType, name, arguments, body) { }

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body)
            : this(accessibility, Static, async, returnType, name, arguments, StringHelper.StringToBodyWriter(body)) { }

        public Method(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, string body)
            : this(accessibility, false, false, returnType, name, arguments, body) { }

        public Method(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body)
            : this(accessibility, false, async, returnType, name, arguments, body) { }

        public Method WithAttribute(Attribute attribute)
        {
            var attributes = new List<Attribute>(Attributes)
            {
                attribute
            };

            return new Method(Accessibility, Static, Async, ReturnType, Name, Arguments, attributes, Body);
        }
    }
}
