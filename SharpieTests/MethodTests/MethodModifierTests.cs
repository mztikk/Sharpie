using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpie;
using Sharpie.Writer.Methods;

namespace SharpieTests.MethodTests
{
    [TestClass]
    public class MethodModifierTests : BaseMethodTest
    {
        [TestMethod]
        public void ContainsStatic() => Contains(GetStaticMethod, "static");
        [TestMethod]
        public void DoesntContainStatic() => DoesNotMatch(GetNonStaticMethod, new Regex(".*static.*"));

        [TestMethod]
        public void ContainsPublic() => Contains(GetPublicMethod, "public");
        [TestMethod]
        public void DoesntContainPublic() => DoesNotMatch(GetPrivateMethod, new Regex(".*public.*"));
        [TestMethod]
        public void ContainsAsync() => Contains(GetStaticAsyncMethod, "async");
        [TestMethod]
        public void ContainsStaticAndAsync()
        {
            Contains(GetStaticAsyncMethod, "static");
            Contains(GetStaticAsyncMethod, "async");
        }
        [DataTestMethod]
        [DataRow("TESTMETHOD")]
        [DataRow("testmethod")]
        [DataRow("test")]
        public void HasName(string name)
        {
            Matches(GetMethodFactoryForName(name), new Regex($".*{name}"));
            EndsWith(GetMethodFactoryForName(name), name);
        }

        private void Contains(Func<Method> methodFactory, string substring) => StringAssert.Contains(ModifierTest(methodFactory), substring);
        private void Matches(Func<Method> methodFactory, Regex pattern) => StringAssert.Matches(ModifierTest(methodFactory), pattern);
        private void DoesNotMatch(Func<Method> methodFactory, Regex pattern) => StringAssert.DoesNotMatch(ModifierTest(methodFactory), pattern);
        private void EndsWith(Func<Method> methodFactory, string s) => StringAssert.EndsWith(ModifierTest(methodFactory), s);

        private static string ModifierTest(Func<Method> methodFactory) => GetString((indentedWriter) => new MethodModifierWriter(indentedWriter, methodFactory()).Make());
    }
}
