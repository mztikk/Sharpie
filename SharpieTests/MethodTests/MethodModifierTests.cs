using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpie;
using Sharpie.Writer.Methods;

namespace SharpieTests.MethodTests
{
    [TestClass]
    public class MethodModifierTests : BaseSharpieTest
    {
        private Method GetStaticMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, true, false, "void", "StaticMethod", Array.Empty<Parameter>(), string.Empty);
        private Method GetNonStaticMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, false, false, "void", "NonStaticMethod", Array.Empty<Parameter>(), string.Empty);

        private Method GetPublicMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Public, true, false, "void", "StaticMethod", Array.Empty<Parameter>(), string.Empty);
        private Method GetPrivateMethod() => new Method(Microsoft.CodeAnalysis.Accessibility.Private, false, false, "void", "NonStaticMethod", Array.Empty<Parameter>(), string.Empty);

        [TestMethod]
        public void ContainsStatic() => Contains(GetStaticMethod, "static");
        [TestMethod]
        public void DoesntContainStatic() => DoesNotMatch(GetNonStaticMethod, new Regex(".*static.*"));

        [TestMethod]
        public void ContainsPublic() => Contains(GetPublicMethod, "public");
        [TestMethod]
        public void DoesntContainPublic() => DoesNotMatch(GetPrivateMethod, new Regex(".*public.*"));

        private void Contains(Func<Method> methodFactory, string substring) => StringAssert.Contains(ModifierTest(methodFactory), substring);
        private void Matches(Func<Method> methodFactory, Regex pattern) => StringAssert.Matches(ModifierTest(methodFactory), pattern);
        private void DoesNotMatch(Func<Method> methodFactory, Regex pattern) => StringAssert.DoesNotMatch(ModifierTest(methodFactory), pattern);

        private static string ModifierTest(Func<Method> methodFactory) => GetString((indentedWriter) => new MethodModifierWriter(indentedWriter, methodFactory()).Make());
    }
}
