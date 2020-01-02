using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class PropertyWriter : BaseWriter
    {
        private readonly List<Property> _properties = new List<Property>();

        public PropertyWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public override bool DidWork { get; protected set; }

        public void AddProperty(Property property) => _properties.Add(property);

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _properties.Count; i++)
            {
                // new line between properties (before everyone except the first one)
                if (i > 0)
                {
                    await WriteLine().ConfigureAwait(false);
                }

                Property property = _properties[i];

                sb.Append(property.Accessibility.ToSharpieString());
                sb.Append(' ');
                sb.Append(property.Type);
                sb.Append(' ');
                sb.Append(property.Name);

                if (property.GetterBody is { } || property.SetterBody is { })
                {
                    await WriteLine(sb.ToString()).ConfigureAwait(false);
                    await WriteLine("{").ConfigureAwait(false);
                    IndentationLevel++;
                    if (property.GetterBody is { })
                    {
                        if (property.GetterAccessibility.HasValue)
                        {
                            await WriteLine(property.GetterAccessibility.Value.ToSharpieString() + " get").ConfigureAwait(false);
                        }
                        else
                        {
                            await WriteLine("get").ConfigureAwait(false);
                        }
                        await WriteLine("{").ConfigureAwait(false);
                        IndentationLevel++;
                        foreach (string line in property.GetterBody.GetLines())
                        {
                            await WriteLine(line).ConfigureAwait(false);
                        }
                        IndentationLevel--;
                        await WriteLine("}").ConfigureAwait(false);
                    }
                    if (property.SetterBody is { })
                    {
                        if (property.SetterAccessibility.HasValue)
                        {
                            await WriteLine(property.SetterAccessibility.Value.ToSharpieString() + " set").ConfigureAwait(false);
                        }
                        else
                        {
                            await WriteLine("set").ConfigureAwait(false);
                        }
                        await WriteLine("{").ConfigureAwait(false);
                        IndentationLevel++;
                        foreach (string line in property.SetterBody.GetLines())
                        {
                            await WriteLine(line).ConfigureAwait(false);
                        }
                        IndentationLevel--;
                        await WriteLine("}").ConfigureAwait(false);
                    }
                    IndentationLevel--;
                    await WriteLine("}").ConfigureAwait(false);
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

                    await WriteLine(sb.ToString()).ConfigureAwait(false);
                }

                sb.Clear();
                DidWork = true;
            }
        }
    }
}
