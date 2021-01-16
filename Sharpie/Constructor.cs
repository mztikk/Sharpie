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
        public readonly ImmutableArray<Parameter> Parameters;
        public readonly Action<IndentedStreamWriter> Body;
        public readonly ImmutableArray<string>? BaseCtorParameters;
        public readonly ImmutableArray<string>? ThisCtorParameters;

        public Constructor(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, Action<IndentedStreamWriter> body)
        {
            Accessibility = accessibility;
            Name = name;
            Parameters = parameters.ToImmutableArray();
            Body = body;

            BaseCtorParameters = null;
            ThisCtorParameters = null;
        }

        public Constructor(Accessibility accessibility, string name, IEnumerable<string> baseCtorParameters, IEnumerable<Parameter> parameters, Action<IndentedStreamWriter> body) : this(accessibility, name, parameters, body) => BaseCtorParameters = baseCtorParameters.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, IEnumerable<string> thisCtorparameters, Action<IndentedStreamWriter> body) : this(accessibility, name, parameters, body) => ThisCtorParameters = thisCtorparameters.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name)
            : this(accessibility, name, Array.Empty<Parameter>(), IndentedStreamWriter.NopWriter) { }

        public Constructor(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, string body)
            : this(accessibility, name, parameters, StringHelper.StringToCall(body)) { }

        public Constructor(Accessibility accessibility, string name, IEnumerable<string> baseCtorparameters, IEnumerable<Parameter> parameters, string body) : this(accessibility, name, parameters, body) => BaseCtorParameters = baseCtorparameters.ToImmutableArray();

        public Constructor(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, IEnumerable<string> thisCtorparameters, string body) : this(accessibility, name, parameters, body) => ThisCtorParameters = thisCtorparameters.ToImmutableArray();
    }
}
