using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sharpie;

namespace SharpieTestSpace
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using (FileStream fs = new FileStream("test.cs", FileMode.Create, FileAccess.ReadWrite))
            {
                IndentedStreamWriter indentedwriter = new IndentedStreamWriter(fs);
                ClassWriter classWriter = new ClassWriter(indentedwriter, "Test");
                classWriter.Namespace.Namespace = "SharpieTEst";
                classWriter.Usings.AddUsing("System");
                classWriter.Usings.AddUsing("System.IO");
                classWriter.AddBaseClass("Object");

                await classWriter.Begin();

                classWriter.AddField(Accessibility.Private, true, "object", "_obj");
                classWriter.AddField(Accessibility.Public, false, "object", "Obj");
                classWriter.AddField<int>(Accessibility.Public, "n");
                classWriter.AddField<StreamWriter>(Accessibility.Protected, "_writer");
                await classWriter.AddConstructor();
                await classWriter.AddConstructor(
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

                await classWriter.End();
            }
        }
    }
}
