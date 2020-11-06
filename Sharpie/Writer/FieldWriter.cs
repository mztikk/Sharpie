using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Sharpie.Writer
{
    public class FieldWriter : BaseWriter
    {
        private readonly List<Field> _fields = new List<Field>();

        public FieldWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public FieldWriter(IndentedStreamWriter writer, IEnumerable<Field> fields) : this(writer) => _fields.AddRange(fields);

        public void AddField(Field field) => _fields.Add(field);

        public void AddField(Accessibility accessibility, bool readOnly, bool isStatic, string type, string name, string? initialValue = null) => AddField(new Field(accessibility, readOnly, isStatic, type, name, initialValue));

        public void AddField(Accessibility accessibility, string type, string name) => AddField(accessibility, false, false, type, name);

        public void AddField<T>(Accessibility accessibility, bool readOnly, bool isStatic, string name) => AddField(accessibility, readOnly, isStatic, typeof(T).CSharpName(), name);

        public void AddField<T>(Accessibility accessibility, string name) => AddField(accessibility, typeof(T).CSharpName(), name);

        protected override bool Start() => false;

        protected override bool Finish()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Field field in _fields)
            {
                var fieldDescription = new List<string>
                {
                    field.Accessibility.ToSharpieString()
                };
                if (field.IsStatic)
                {
                    fieldDescription.Add("static");
                }
                if (field.ReadOnly)
                {
                    fieldDescription.Add("readonly");
                }
                if (field.IsConst)
                {
                    fieldDescription.Add("const");
                }
                fieldDescription.Add(field.Type);
                fieldDescription.Add(field.Name);

                sb.Append(string.Join(" ", fieldDescription));

                if (field.InitialValue is { })
                {
                    sb.Append(" = ");
                    sb.Append(field.InitialValue);
                }
                sb.Append(";");
                WriteLine(sb.ToString());

                sb.Clear();
            }

            return _fields.Count > 0;
        }
    }
}
