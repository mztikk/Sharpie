using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct Constructor
    {
        public readonly Accessibility Accessibility;
        public readonly string Name;
        public readonly ImmutableArray<Argument> Arguments;
        public readonly Action<IndentedStreamWriter> Body;
        public readonly ImmutableArray<string>? BaseCtorArguments;
        public readonly ImmutableArray<string>? ThisCtorArguments;

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body)
        {
            Accessibility = accessibility;
            Name = name;
            Arguments = arguments.ToImmutableArray();
            Body = body;

            BaseCtorArguments = null;
            ThisCtorArguments = null;
        }

        public Constructor(Accessibility accessibility, string name, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) : this(accessibility, name, arguments, body) => BaseCtorArguments = baseCtorArguments.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, Action<IndentedStreamWriter> body) : this(accessibility, name, arguments, body) => ThisCtorArguments = thisCtorArguments.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name)
            : this(accessibility, name, Array.Empty<Argument>(), IndentedStreamWriter.NopWriter) { }

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, string body)
            : this(accessibility, name, arguments, StringHelper.StringToCall(body)) { }

        public Constructor(Accessibility accessibility, string name, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, string body) : this(accessibility, name, arguments, body) => BaseCtorArguments = baseCtorArguments.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, string body) : this(accessibility, name, arguments, body) => ThisCtorArguments = thisCtorArguments.ToImmutableArray();
    }
}
