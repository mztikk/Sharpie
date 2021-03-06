﻿using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Sharpie.Writer
{
    public class PropertyWriter : BaseWriter
    {
        private readonly List<Property> _properties = new List<Property>();

        public PropertyWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public PropertyWriter(IndentedStreamWriter writer, IEnumerable<Property> properties) : this(writer) => _properties.AddRange(properties);

        public void AddProperty(Property property) => _properties.Add(property);

        public void AddProperty<T>(Accessibility accessibility, string name) => AddProperty(new Property(accessibility, typeof(T).CSharpName(), name));

        protected override bool Start() => false;

        protected override bool Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _properties.Count; i++)
            {
                // new line between properties (before everyone except the first one)
                if (i > 0)
                {
                    WriteLine();
                }

                Property property = _properties[i];

                sb.Append(property.Accessibility.ToSharpieString());
                sb.Append(' ');
                sb.Append(property.Type);
                sb.Append(' ');
                sb.Append(property.Name);

                if (property.GetterBody is { } || property.SetterBody is { })
                {
                    WriteLine(sb.ToString());
                    OpenBrackets();
                    if (property.GetterBody is { })
                    {
                        if (property.GetterAccessibility.HasValue)
                        {
                            WriteLine(property.GetterAccessibility.Value.ToSharpieString() + " get");
                        }
                        else
                        {
                            WriteLine("get");
                        }
                        OpenBrackets();

                        var bodyWriter = new BodyWriter(_writer);
                        property.GetterBody(bodyWriter);

                        CloseBrackets();
                    }
                    if (property.SetterBody is { })
                    {
                        if (property.SetterAccessibility.HasValue)
                        {
                            WriteLine(property.SetterAccessibility.Value.ToSharpieString() + " set");
                        }
                        else
                        {
                            WriteLine("set");
                        }
                        OpenBrackets();

                        var bodyWriter = new BodyWriter(_writer);
                        property.SetterBody(bodyWriter);

                        CloseBrackets();
                    }
                    CloseBrackets();
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

                    WriteLine(sb.ToString());
                }

                sb.Clear();
            }

            return _properties.Count > 0;
        }
    }
}
