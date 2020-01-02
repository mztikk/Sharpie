using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpie.Writer
{
    public class FieldWriter : BaseWriter
    {
        private readonly List<Field> _fields = new List<Field>();

        public FieldWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

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
            foreach (Field field in _fields)
            {
                await WriteLine(field.Accessibility.ToSharpieString() + (field.ReadOnly ? " readonly " : " ") + field.Type + " " + field.Name + (field.InitialValue is { } ? " = " + field.InitialValue : "") + ";").ConfigureAwait(false);
                DidWork = true;
            }
        }
    }
}
