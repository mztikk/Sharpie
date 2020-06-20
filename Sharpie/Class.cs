using System;
using System.Collections.Generic;
using Sharpie.Writer;

namespace Sharpie
{
    public class Class : IEquatable<Class>
    {
        public Class() : this(Internals.GetIdentifierName()) { }
        public Class(string className) => ClassName = className;

        internal readonly HashSet<string> _usings = new HashSet<string>();
        internal readonly HashSet<string> _baseClasses = new HashSet<string>();
        internal readonly List<Constructor> _ctors = new List<Constructor>();
        internal readonly List<Method> _methods = new List<Method>();
        internal readonly List<Property> _properties = new List<Property>();
        internal readonly List<Field> _fields = new List<Field>();

        public Accessibility Accessibility { get; set; }

        public bool Static { get; set; }

        public string Namespace { get; set; }

        public string ClassName { get; set; }

        public Class SetClassName(string className)
        {
            ClassName = className;
            return this;
        }

        public Class SetNamespace(string namespaceName)
        {
            Namespace = namespaceName;
            return this;
        }

        public Class WithAccessibility(Accessibility accessibility)
        {
            Accessibility = accessibility;
            return this;
        }

        public Class SetStatic(bool isStatic)
        {
            Static = isStatic;
            return this;
        }

        public Class WithConstructor(Constructor ctor)
        {
            _ctors.Add(ctor);
            return this;
        }

        public Class WithConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, arguments, body));

        public Class WithConstructor(Accessibility accessibility = Accessibility.Public) => WithConstructor(accessibility, Array.Empty<Argument>(), string.Empty);

        public Class WithConstructor(Accessibility accessibility, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, baseCtorArguments, arguments, body));
        public Class WithConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, arguments, thisCtorArguments, body));

        public Class WithConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, string body) => WithConstructor(new Constructor(accessibility, ClassName, arguments, body));

        public Class WithConstructor(Accessibility accessibility, IEnumerable<string> baseCtorArguments, IEnumerable<Argument> arguments, string body) => WithConstructor(new Constructor(accessibility, ClassName, baseCtorArguments, arguments, body));
        public Class WithConstructor(Accessibility accessibility, IEnumerable<Argument> arguments, IEnumerable<string> thisCtorArguments, string body) => WithConstructor(new Constructor(accessibility, ClassName, arguments, thisCtorArguments, body));

        public Class WithMethod(Method method)
        {
            _methods.Add(method);
            return this;
        }

        public Class WithMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => WithMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        public Class WithMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => WithMethod(accessibility, false, false, returnType, name, arguments, body);

        public Class WithMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, Action<BodyWriter> body) => WithMethod(accessibility, false, async, returnType, name, arguments, body);

        public Class WithMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body) => WithMethod(new Method(accessibility, Static, async, returnType, name, arguments, body));

        public Class WithMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Argument> arguments, string body) => WithMethod(accessibility, false, false, returnType, name, arguments, body);

        public Class WithMethod(Accessibility accessibility, string returnType, string name, string body) => WithMethod(accessibility, false, false, returnType, name, Array.Empty<Argument>(), body);

        public Class WithMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Argument> arguments, string body) => WithMethod(accessibility, false, async, returnType, name, arguments, body);

        public Class WithProperty(Property property)
        {
            _properties.Add(property);
            return this;
        }

        public Class WithProperty<T>(Accessibility accessibility, string name) => WithProperty(new Property(accessibility, typeof(T).CSharpName(), name));

        public Class WithField(Field field)
        {
            _fields.Add(field);
            return this;
        }

        public Class WithField(Accessibility accessibility, bool readOnly, string type, string name, string? initialValue = null) => WithField(new Field(accessibility, readOnly, type, name, initialValue));
        public Class WithField(Accessibility accessibility, string type, string name) => WithField(accessibility, false, type, name);
        public Class WithField<T>(Accessibility accessibility, bool readOnly, string name) => WithField(accessibility, readOnly, typeof(T).CSharpName(), name);
        public Class WithField<T>(Accessibility accessibility, string name) => WithField(accessibility, typeof(T).CSharpName(), name);

        public Class WithUsing(string usingName)
        {
            _usings.Add(usingName);
            return this;
        }

        public Class WithBaseClass(string className)
        {
            _baseClasses.Add(className);
            return this;
        }

        public override int GetHashCode() => HashCode.Combine(Namespace, ClassName);
        public bool Equals(Class other) => Namespace.Equals(other.Namespace) && ClassName.Equals(other.ClassName);
    }
}
