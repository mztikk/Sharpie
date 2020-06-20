using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using RFReborn.Extensions;

namespace Sharpie.Writer
{
    public class ClassWriterTest
    {
        private readonly Class _class;

        public ClassWriterTest(Class c) => _class = c;

        private IEnumerable<string> GetUsingStatements()
        {
            foreach (string item in _class.Usings)
            {
                yield return "using " + item + ";";
            }
        }

        private string GetUsing() => string.Join(Environment.NewLine, GetUsingStatements());

        private string GetInheritance()
        {
            ImmutableList<string>? baseClasses = _class.BaseClasses;

            if (baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", baseClasses);
        }

        private void WriteFields(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            ImmutableList<Field>? fields = _class.Fields;

            foreach (Field field in fields)
            {
                sb.Append(field.Accessibility.ToSharpieString());
                if (field.ReadOnly)
                {
                    sb.Append(" readonly ");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(field.Type);
                sb.Append(" ");
                sb.Append(field.Name);
                if (field.InitialValue is { })
                {
                    sb.Append(" = ");
                    sb.Append(field.InitialValue);
                }
                sb.Append(";");
                writer.WriteLine(sb.ToString());

                sb.Clear();
            }
        }

        private void WriteCtors(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            ImmutableList<Constructor>? ctors = _class.Ctors;

            for (int i = 0; i < ctors.Count; i++)
            {
                // new line between ctors (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                sb.Append(ctors[i].Accessibility.ToSharpieString());
                sb.Append(" ");
                sb.Append(ctors[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", ctors[i].Arguments));
                sb.Append(")");
                if (ctors[i].BaseCtorArguments is { })
                {
                    sb.Append(" : base(");
                    sb.Append(string.Join(", ", ctors[i].BaseCtorArguments));
                    sb.Append(")");
                }
                else if (ctors[i].ThisCtorArguments is { })
                {
                    sb.Append(" : this(");
                    sb.Append(string.Join(", ", ctors[i].ThisCtorArguments));
                    sb.Append(")");
                }
                writer.WriteLine(sb.ToString());
                sb.Clear();
                writer.WriteLine("{");

                writer.IndentationLevel++;

                ctors[i].Body(writer);
                writer.IndentationLevel--;

                writer.WriteLine("}");
                sb.Clear();
            }
        }

        private void WriteProperties(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            ImmutableList<Property>? properties = _class.Properties;

            for (int i = 0; i < properties.Count; i++)
            {
                // new line between properties (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                Property property = properties[i];

                sb.Append(property.Accessibility.ToSharpieString());
                sb.Append(' ');
                sb.Append(property.Type);
                sb.Append(' ');
                sb.Append(property.Name);

                if (property.GetterBody is { } || property.SetterBody is { })
                {
                    writer.WriteLine(sb.ToString());
                    writer.WriteLine("{");
                    writer.IndentationLevel++;
                    if (property.GetterBody is { })
                    {
                        if (property.GetterAccessibility.HasValue)
                        {
                            writer.WriteLine(property.GetterAccessibility.Value.ToSharpieString() + " get");
                        }
                        else
                        {
                            writer.WriteLine("get");
                        }
                        writer.WriteLine("{");
                        writer.IndentationLevel++;

                        var bodyWriter = new BodyWriter(writer);
                        property.GetterBody(bodyWriter);

                        writer.IndentationLevel--;
                        writer.WriteLine("}");
                    }
                    if (property.SetterBody is { })
                    {
                        if (property.SetterAccessibility.HasValue)
                        {
                            writer.WriteLine(property.SetterAccessibility.Value.ToSharpieString() + " set");
                        }
                        else
                        {
                            writer.WriteLine("set");
                        }
                        writer.WriteLine("{");
                        writer.IndentationLevel++;

                        var bodyWriter = new BodyWriter(writer);
                        property.SetterBody(bodyWriter);

                        writer.IndentationLevel--;
                        writer.WriteLine("}");
                    }
                    writer.IndentationLevel--;
                    writer.WriteLine("}");
                }
                else
                {
                    sb.Append(" { ");
                    if (property.GetterAccessibility.HasValue)
                    {
                        sb.Append(property.GetterAccessibility.Value.ToSharpieString());
                        sb.Append(" ");
                    }

                    sb.Append("get; ");

                    if (property.SetterAccessibility.HasValue)
                    {
                        sb.Append(property.SetterAccessibility.Value.ToSharpieString());
                        sb.Append(" ");
                    }

                    sb.Append("set; }");

                    if (property.InitialValue is { })
                    {
                        sb.Append(" = ");
                        sb.Append(property.InitialValue);
                        sb.Append(";");
                    }

                    writer.WriteLine(sb.ToString());
                }

                sb.Clear();
            }
        }

        private void WriteMethods(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            ImmutableList<Method>? methods = _class.Methods;

            for (int i = 0; i < methods.Count; i++)
            {
                // new line between methods (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                sb.Append(methods[i].Accessibility.ToSharpieString());
                if (methods[i].Static)
                {
                    sb.Append(" static");
                }
                if (methods[i].Async)
                {
                    sb.Append(" async");
                }
                sb.Append(" ");
                sb.Append(methods[i].ReturnType);
                sb.Append(" ");
                sb.Append(methods[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", methods[i].Arguments));
                sb.Append(")");
                writer.WriteLine(sb.ToString());
                sb.Clear();
                writer.WriteLine("{");

                writer.IndentationLevel++;

                var bodyWriter = new BodyWriter(writer);

                methods[i].Body(bodyWriter);
                writer.IndentationLevel--;

                writer.WriteLine("}");
            }
        }

        public void Write(Stream stream)
        {
            using (var writer = new IndentedStreamWriter(stream))
            {
                // Usings
                writer.WriteLine(GetUsing());
                if (_class.Usings.Any())
                {
                    writer.WriteLine();
                }

                // Namespace begin
                writer.WriteLine("namespace " + _class.Namespace);
                writer.WriteLine("{");
                writer.IndentationLevel++;

                // Class begin
                writer.WriteLine(_class.Accessibility.Value.ToSharpieString() + " class " + _class.ClassName + GetInheritance());
                writer.WriteLine("{");
                writer.IndentationLevel++;

                // (Fields);
                WriteFields(writer);
                // (Ctors);
                WriteCtors(writer);
                // (Properties);
                WriteProperties(writer);
                // (Methods);
                WriteMethods(writer);

                // Class end
                writer.IndentationLevel--;
                writer.WriteLine("}");

                // Namespace end
                writer.IndentationLevel--;
                writer.WriteLine("}");
            }
        }

        public string Write()
        {
            using (var stream = new MemoryStream())
            {
                Write(stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
