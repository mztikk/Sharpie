using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sharpie;
using Sharpie.Writer;

namespace SharpieTestSpace
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using (FileStream fs = new FileStream("test.cs", FileMode.Create, FileAccess.ReadWrite))
            {
                IndentedStreamWriter indentedwriter = new IndentedStreamWriter(fs);
                ClassWriter classWriter = new ClassWriter(indentedwriter, "Test", "SharpieTEst");
                classWriter.Usings.AddUsing("System");
                classWriter.Usings.AddUsing("System.IO");
                classWriter.AddBaseClass("Object");

                await classWriter.Begin().ConfigureAwait(false);

                classWriter.Fields.AddField(Accessibility.Private, true, "object", "_obj");
                classWriter.Fields.AddField(Accessibility.Public, false, "object", "Obj");
                classWriter.Fields.AddField<int>(Accessibility.Public, "n");
                classWriter.Fields.AddField<StreamWriter>(Accessibility.Protected, "_writer");
                classWriter.Ctors.AddConstructor();
                classWriter.Ctors.AddConstructor(
                    Accessibility.Public,
                    new List<Argument>()
                    {
                        new Argument("object", "obj"),
                        new Argument("object", "obj2")
                    },

                    "_obj = obj;"
                    + Environment.NewLine +
                    "Obj = obj2;"
                    );
                classWriter.Ctors.AddConstructor(
                    Accessibility.Public,
                    new List<Argument>()
                    {
                        new Argument("object", "obj"),
                        new Argument("object", "obj2")
                    },
                    new List<string>()
                    {
                        "obj", "obj2"
                    },
                    "n = 5;"
                    );

                classWriter.Properties.AddProperty<string>(Accessibility.Public, "TestPropString");
                classWriter.Fields.AddField<string>(Accessibility.Private, "_fullPropTest");
                classWriter.Properties.AddProperty(new Property(Accessibility.Public, "string", "FullPropTest", null, "return _fullPropTest;", null, "_fullPropTest = value;", null));
                classWriter.Properties.AddProperty(new Property(Accessibility.Public, "int", "GetterOnlyTest", null, "return n;", null, null, null));
                classWriter.Properties.AddProperty(new Property(Accessibility.Public, "string", "FullPropTestWithAccess", Accessibility.Public, "return _fullPropTest;", Accessibility.Protected, "_fullPropTest = value;", null));
                classWriter.Methods.AddMethod(Accessibility.Public, "string", "Get5", Array.Empty<Argument>(), (writer) => writer.WriteLine("return \"5\";"));

                await classWriter.End().ConfigureAwait(false);
            }
        }
    }
}
