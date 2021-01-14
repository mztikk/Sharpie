using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sharpie
{
    public readonly struct Attribute
    {
        public static readonly Attribute InlineAttribute = new Attribute("System.Runtime.CompilerServices.MethodImpl",
                                                                         new string[] { "System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining" });

        public readonly string Name;
        public readonly ImmutableArray<string> Parameters;

        public Attribute(string name, IEnumerable<string> parameters)
        {
            Name = name;
            Parameters = parameters.ToImmutableArray();
        }

        public Attribute(string name) : this(name, Array.Empty<string>()) { }
    }
}
