﻿using System;
using System.Collections.Generic;
using Sharpie.Writer;

namespace Sharpie
{
    public class Method
    {
        public readonly Accessibility Accessibility;
        public readonly bool Static;
        public readonly bool Async;
        public readonly string ReturnType;
        public readonly string Name;
        public readonly IEnumerable<Argument> Arguments;
        public readonly Action<IndentedStreamWriter> Body;

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body)
        {
            Accessibility = accessibility;
            this.Static = Static;
            Async = async;
            ReturnType = returnType;
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public Method(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body)
            : this(accessibility, false, false, returnType, name, arguments, body) { }

        public Method(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body)
            : this(accessibility, false, async, returnType, name, arguments, body) { }
    }
}
