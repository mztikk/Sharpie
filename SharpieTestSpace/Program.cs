using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpie;
using Sharpie.Writer;

namespace SharpieTestSpace
{
    internal class Program
    {
        private static async Task Main(string[] args)
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
                            new Argument("object", "obj2")
                        },
                        new List<string>() { "obj", "obj2" },
                        "n = 5;")
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
                    .WithMethod(Accessibility.Public, "string", "Get5", "return \"5\";");


            using (var fs = new FileStream("test.cs", FileMode.Create, FileAccess.ReadWrite))
            {
                await ClassWriter.WriteAsync(c, fs).ConfigureAwait(false);
            }
        }
    }
}
