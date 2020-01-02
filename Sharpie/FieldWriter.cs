﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpie
{
    public class FieldWriter : BaseWriter
    {
        private readonly List<Field> _fields = new List<Field>();

        public FieldWriter(IndentedStreamWriter writer) : base(writer)
        {
        }

        public void AddField(Accessibility accessibility, bool readOnly, string type, string name, string? initialValue = null) => _fields.Add(new Field(accessibility, readOnly, type, name, initialValue));

        public void AddField(Accessibility accessibility, string type, string name) => AddField(accessibility, false, type, name);

        public void AddField<T>(Accessibility accessibility, bool readOnly, string name) => AddField(accessibility, readOnly, TypeHelper.GetCSharpTypeName(typeof(T).Name), name);

        public void AddField<T>(Accessibility accessibility, string name) => AddField(accessibility, TypeHelper.GetCSharpTypeName(typeof(T).Name), name);

        public override Task Begin() =>
            // NOP
            Task.CompletedTask;

        public override async Task End()
        {
            foreach (Field field in _fields)
            {
                await WriteLine(field.Accessibility.ToSharpieString() + (field.ReadOnly ? " readonly " : " ") + field.Type + " " + field.Name + (field.InitialValue is { } ? " = " + field.InitialValue : "") + ";").ConfigureAwait(false);
            }
        }
    }
}
