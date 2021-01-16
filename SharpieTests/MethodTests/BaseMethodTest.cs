using System;
using Sharpie;

namespace SharpieTests.MethodTests
{
    public class BaseMethodTest : BaseSharpieTest
    {
        protected static Method GetStaticMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, true, false, "void", "StaticMethod", Array.Empty<Parameter>(), string.Empty);
        protected static Method GetNonStaticMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, false, false, "void", "NonStaticMethod", Array.Empty<Parameter>(), string.Empty);

        protected static Method GetPublicMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, true, false, "void", "StaticMethod", Array.Empty<Parameter>(), string.Empty);
        protected static Method GetPrivateMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Private, false, false, "void", "NonStaticMethod", Array.Empty<Parameter>(), string.Empty);
    }
}
