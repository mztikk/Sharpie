using System.Collections.Generic;

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
        public readonly string Body;

        public Method(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body)
        {
            Accessibility = accessibility;
            this.Static = Static;
            Async = async;
            ReturnType = returnType;
            Name = name;
            Arguments = arguments;
            Body = body;
        }
    }
}
