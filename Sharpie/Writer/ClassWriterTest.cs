using System;
using System.Collections.Generic;
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
            foreach (string item in _class._usings)
            {
                yield return "using " + item + ";";
            }
        }

        private string GetUsing() => string.Join(Environment.NewLine, GetUsingStatements());

        private string GetInheritance()
        {
            if (_class._baseClasses.Count == 0)
            {
                return string.Empty;
            }

            return " : " + string.Join(", ", _class._baseClasses);
        }

        private void WriteFields(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Field field in _class._fields)
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

            for (int i = 0; i < _class._ctors.Count; i++)
            {
                // new line between ctors (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                sb.Append(_class._ctors[i].Accessibility.ToSharpieString());
                sb.Append(" ");
                sb.Append(_class._ctors[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", _class._ctors[i].Arguments));
                sb.Append(")");
                if (_class._ctors[i].BaseCtorArguments is { })
                {
                    sb.Append(" : base(");
                    sb.Append(string.Join(", ", _class._ctors[i].BaseCtorArguments));
                    sb.Append(")");
                }
                else if (_class._ctors[i].ThisCtorArguments is { })
                {
                    sb.Append(" : this(");
                    sb.Append(string.Join(", ", _class._ctors[i].ThisCtorArguments));
                    sb.Append(")");
                }
                writer.WriteLine(sb.ToString());
                sb.Clear();
                writer.WriteLine("{");

                writer.IndentationLevel++;

                _class._ctors[i].Body(writer);
                writer.IndentationLevel--;

                writer.WriteLine("}");
                sb.Clear();
            }
        }

        private void WriteProperties(IndentedStreamWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _class._properties.Count; i++)
            {
                // new line between properties (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                Property property = _class._properties[i];

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

                        property.GetterBody(writer);
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

                        property.SetterBody(writer);
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

            for (int i = 0; i < _class._methods.Count; i++)
            {
                // new line between methods (before everyone except the first one)
                if (i > 0)
                {
                    writer.WriteLine();
                }

                sb.Append(_class._methods[i].Accessibility.ToSharpieString());
                if (_class._methods[i].Static)
                {
                    sb.Append(" static");
                }
                if (_class._methods[i].Async)
                {
                    sb.Append(" async");
                }
                sb.Append(" ");
                sb.Append(_class._methods[i].ReturnType);
                sb.Append(" ");
                sb.Append(_class._methods[i].Name);
                sb.Append("(");
                sb.Append(string.Join(", ", _class._methods[i].Arguments));
                sb.Append(")");
                writer.WriteLine(sb.ToString());
                sb.Clear();
                writer.WriteLine("{");

                writer.IndentationLevel++;

                _class._methods[i].Body(writer);
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
                if (_class._usings.Any())
                {
                    writer.WriteLine();
                }

                // Namespace begin
                writer.WriteLine("namespace " + _class.Namespace);
                writer.WriteLine("{");
                writer.IndentationLevel++;

                // Class begin
                writer.WriteLine(_class.Accessibility.ToSharpieString() + " class " + _class.ClassName + GetInheritance());
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
