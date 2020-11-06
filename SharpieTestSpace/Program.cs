using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Sharpie;
using Sharpie.Writer;

namespace SharpieTestSpace
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Class c = new Class("Test")
                    .WithUsing("System")
                    .WithUsing("System.IO")
                    .WithBaseClass("Object")
                    .SetNamespace("SharpieTEst")
                    .WithField(Accessibility.Private, true, false, "object", "_obj")
                    .WithField(Accessibility.Public, false, false, "object", "Obj")
                    .WithField<int>(Accessibility.Public, "n")
                    .WithField<StreamWriter>(Accessibility.Protected, "_writer")
                    .WithConstructor()
                    .WithConstructor(
                        Accessibility.Public,
                        new List<Argument>()
                        {
                            new Argument("object", "obj"),
                            new Argument("object", "obj2")
                        },
                        "_obj = obj;" + Environment.NewLine + "Obj = obj2;")
                    .WithConstructor(
                        Accessibility.Public,
                        new List<Argument>()
                        {
                            new Argument("object", "obj"),
                            new Argument("object", "obj2"),
                            new Argument("int", "n"),
                        },
                        new List<string>() { "obj", "obj2" },
                        "this.n = n;")
                    .WithProperty<string>(Accessibility.Public, "TestPropString")
                    .WithField<string>(Accessibility.Private, "_fullPropTest")
                    .WithProperty(new Property(Accessibility.Public, "string", "FullPropTest", null, "return _fullPropTest;", null, "_fullPropTest = value;", null))
                    .WithProperty(new Property(Accessibility.Public, "int", "GetterOnlyTest", null, "return n;", null, null, null))
                    .WithProperty(new Property(
                        Accessibility.Public,
                        "string",
                        "FullPropTestWithAccess",
                        null,
                        "return _fullPropTest;",
                        Accessibility.Protected,
                        "_fullPropTest = value;",
                        null))
                    .WithMethod(Accessibility.Public, "string", "Get5", "return \"5\";")
                    .WithMethod(new Method(Accessibility.Public, "string", "Switch5", new Argument[] { new Argument("int", "n", "5") }, (bodyWriter) =>
                     {
                         bodyWriter.WriteSwitchCaseStatement(new SwitchCaseStatement("n", new CaseStatement[] {
                            new CaseStatement("0", "return \"0\";"),
                            new CaseStatement("1", "return \"1\";"),
                            new CaseStatement("2", "return \"2\";"),
                            new CaseStatement("3", "return \"3\";"),
                            new CaseStatement("4", "return \"4\";"),
                            new CaseStatement("5", "return \"5\";"),
                         },
                         "throw new ArgumentOutOfRangeException();"));
                     }));


            using (var fs = new FileStream("test.cs", FileMode.Create, FileAccess.ReadWrite))
            {
                ClassWriter.Write(c, fs);
            }
        }
    }
}
