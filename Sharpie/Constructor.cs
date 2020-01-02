using System;
using System.Collections.Generic;

namespace Sharpie
{
    public class Constructor
    {
        public readonly Accessibility Accessibility;
        public readonly string Name;
        public readonly IEnumerable<Argument> Arguments;
        public readonly string Body;
        public readonly IEnumerable<string>? BaseCtorArguments;
        public readonly IEnumerable<string>? ThisCtorArguments;

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, string body)
        {
            Accessibility = accessibility;
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public Constructor(Accessibility accessibility, string name, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, string body) : this(accessibility, name, arguments, body) => BaseCtorArguments = baseCtorArguments;

        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, string body) : this(accessibility, name, arguments, body) => ThisCtorArguments = thisCtorArguments;

        public Constructor(Accessibility accessibility, string name)
            : this(accessibility, name, Array.Empty<Argument>(), string.Empty) { }
    }
}
