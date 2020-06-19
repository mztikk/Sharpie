using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class FieldWriter : BaseWriter
    {
        private readonly List<Field> _fields = new List<Field>();

        public FieldWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public FieldWriter(IndentedStreamWriter writer, IEnumerable<Field> fields) : this(writer) => _fields.AddRange(fields);

        public override bool DidWork { get; protected set; }

        public void AddField(Field field) => _fields.Add(field);

        public void AddField(Accessibility accessibility, bool readOnly, string type, string name, string? initialValue = null) => AddField(new Field(accessibility, readOnly, type, name, initialValue));

        public void AddField(Accessibility accessibility, string type, string name) => AddField(accessibility, false, type, name);

        public void AddField<T>(Accessibility accessibility, bool readOnly, string name) => AddField(accessibility, readOnly, typeof(T).CSharpName(), name);

        public void AddField<T>(Accessibility accessibility, string name) => AddField(accessibility, typeof(T).CSharpName(), name);

        protected override Task Start() =>
            // NOP
            Task.CompletedTask;

        protected override async Task Finish()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Field field in _fields)
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
                await WriteLineAsync(sb.ToString()).ConfigureAwait(false);

                sb.Clear();
                DidWork = true;
            }
        }
    }
}
