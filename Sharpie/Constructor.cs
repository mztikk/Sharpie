using System.Collections.Generic;

namespace Sharpie
{
    public class Constructor
    {
        public Constructor(Accessibility accessibility, string name, IEnumerable<Argument> arguments, string body)
        {
            Accessibility = accessibility;
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public readonly Accessibility Accessibility;
        public readonly string Name;
        public readonly IEnumerable<Argument> Arguments;
        public readonly string Body;
    }
}
