﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct Class : IEquatable<Class>
    {
        private readonly HashSet<string> _usings;
        private readonly HashSet<string> _baseClasses;
        private readonly List<Constructor> _ctors;
        private readonly List<Method> _methods;
        private readonly List<Property> _properties;
        private readonly List<Field> _fields;

        public readonly Accessibility? Accessibility;
        public readonly bool Static;
        public readonly bool Partial;
        public readonly string? Namespace;
        public readonly string ClassName;

        public Class(
            string className,
            string? nameSpace,
            Accessibility? accessibility,
            bool isStatic,
            bool isPartial,
            IEnumerable<string> usings,
            IEnumerable<string> baseClasses,
            IEnumerable<Constructor> ctors,
            IEnumerable<Method> methods,
            IEnumerable<Property> properties,
            IEnumerable<Field> fields)
        {
            ClassName = className;
            Namespace = nameSpace;
            Accessibility = accessibility;
            Static = isStatic;
            Partial = isPartial;
            _usings = new HashSet<string>(usings);
            _baseClasses = new HashSet<string>(baseClasses);
            _ctors = new List<Constructor>(ctors);
            _methods = new List<Method>(methods);
            _properties = new List<Property>(properties);
            _fields = new List<Field>(fields);
        }

        public Class(string className) : this(
            className,
            null,
            null,
            false,
            false,
            Array.Empty<string>(),
            Array.Empty<string>(),
            Array.Empty<Constructor>(),
            Array.Empty<Method>(),
            Array.Empty<Property>(),
            Array.Empty<Field>())
        { }

        public ImmutableArray<string> Usings => _usings.ToImmutableArray();
        public ImmutableArray<string> BaseClasses => _baseClasses.ToImmutableArray();
        public ImmutableArray<Constructor> Ctors => _ctors.ToImmutableArray();
        public ImmutableArray<Method> Methods => _methods.ToImmutableArray();
        public ImmutableArray<Property> Properties => _properties.ToImmutableArray();
        public ImmutableArray<Field> Fields => _fields.ToImmutableArray();

        public Class SetClassName(string className) => new Class(className, Namespace, Accessibility, Static, Partial, _usings, _baseClasses, _ctors, _methods, _properties, _fields);

        public Class SetNamespace(string namespaceName) => new Class(ClassName, namespaceName, Accessibility, Static, Partial, _usings, _baseClasses, _ctors, _methods, _properties, _fields);
        public Class WithAccessibility(Accessibility accessibility) => new Class(ClassName, Namespace, accessibility, Static, Partial, _usings, _baseClasses, _ctors, _methods, _properties, _fields);

        public Class SetStatic(bool isStatic) => new Class(ClassName, Namespace, Accessibility, isStatic, Partial, _usings, _baseClasses, _ctors, _methods, _properties, _fields);
        public Class SetPartial(bool isPartial) => new Class(ClassName, Namespace, Accessibility, Static, isPartial, _usings, _baseClasses, _ctors, _methods, _properties, _fields);

        public Class WithConstructor(Constructor ctor)
        {
            _ctors.Add(ctor);
            return this;
        }

        public Class WithConstructor(Accessibility accessibility, IEnumerable<Parameter> parameters, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, parameters, body));

        public Class WithConstructor(Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.Public) => WithConstructor(accessibility, Array.Empty<Parameter>(), string.Empty);

        public Class WithConstructor(Accessibility accessibility, IEnumerable<string> baseCtorparameters, IEnumerable<Parameter> parameters, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, baseCtorparameters, parameters, body));
        public Class WithConstructor(Accessibility accessibility, IEnumerable<Parameter> parameters, IEnumerable<string> thisCtorparameters, Action<IndentedStreamWriter> body) => WithConstructor(new Constructor(accessibility, ClassName, parameters, thisCtorparameters, body));

        public Class WithConstructor(Accessibility accessibility, IEnumerable<Parameter> parameters, string body) => WithConstructor(new Constructor(accessibility, ClassName, parameters, body));

        public Class WithConstructor(Accessibility accessibility, IEnumerable<string> baseCtorparameters, IEnumerable<Parameter> parameters, string body) => WithConstructor(new Constructor(accessibility, ClassName, baseCtorparameters, parameters, body));
        public Class WithConstructor(Accessibility accessibility, IEnumerable<Parameter> parameters, IEnumerable<string> thisCtorparameters, string body) => WithConstructor(new Constructor(accessibility, ClassName, parameters, thisCtorparameters, body));

        public Class WithMethods(IEnumerable<Method> methods)
        {
            _methods.AddRange(methods);

            return this;
        }

        public Class WithMethod(Method method)
        {
            _methods.Add(method);
            return this;
        }

        public Class WithMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body) => WithMethod(new Method(accessibility, Static, async, returnType, name, parameters, body));
        public Class WithMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body) => WithMethod(accessibility, false, false, returnType, name, parameters, body);
        public Class WithMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body) => WithMethod(accessibility, false, async, returnType, name, parameters, body);
        public Class WithMethod(Accessibility accessibility, bool Static, bool async, string returnType, string name, IEnumerable<Parameter> parameters, string body) => WithMethod(new Method(accessibility, Static, async, returnType, name, parameters, body));
        public Class WithMethod(Accessibility accessibility, string returnType, string name, IEnumerable<Parameter> parameters, string body) => WithMethod(accessibility, false, false, returnType, name, parameters, body);
        public Class WithMethod(Accessibility accessibility, string returnType, string name, string body) => WithMethod(accessibility, false, false, returnType, name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, bool async, string returnType, string name, IEnumerable<Parameter> parameters, string body) => WithMethod(accessibility, false, async, returnType, name, parameters, body);
        public Class WithMethod(Accessibility accessibility, string name, Action<BodyWriter> body) => WithMethod(accessibility, false, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, string name, string body) => WithMethod(accessibility, false, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, string body) => WithMethod(accessibility, false, false, "void", name, parameters, body);
        public Class WithMethod(Accessibility accessibility, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body) => WithMethod(accessibility, false, false, "void", name, parameters, body);
        public Class WithMethod(string name, string body) => WithMethod(Microsoft.CodeAnalysis.Accessibility.Protected, false, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(string name, Action<BodyWriter> body) => WithMethod(Microsoft.CodeAnalysis.Accessibility.Protected, false, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, bool isStatic, string name, string body) => WithMethod(accessibility, isStatic, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, bool isStatic, string name, Action<BodyWriter> body) => WithMethod(accessibility, isStatic, false, "void", name, Array.Empty<Parameter>(), body);
        public Class WithMethod(Accessibility accessibility, bool isStatic, string name, IEnumerable<Parameter> parameters, string body) => WithMethod(accessibility, isStatic, false, "void", name, parameters, body);
        public Class WithMethod(Accessibility accessibility, bool isStatic, string name, IEnumerable<Parameter> parameters, Action<BodyWriter> body) => WithMethod(accessibility, isStatic, false, "void", name, parameters, body);

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

        public Class WithField(Accessibility accessibility, bool readOnly, bool isStatic, string type, string name, string? initialValue = null) => WithField(new Field(accessibility, readOnly, isStatic, type, name, initialValue));
        public Class WithField(Accessibility accessibility, bool isConst, string type, string name, string? initialValue = null) => WithField(new Field(accessibility, isConst, type, name, initialValue));
        public Class WithField(Accessibility accessibility, string type, string name) => WithField(accessibility, false, false, type, name);
        public Class WithField<T>(Accessibility accessibility, bool readOnly, string name) => WithField(accessibility, readOnly, false, typeof(T).CSharpName(), name);
        public Class WithField<T>(Accessibility accessibility, string name) => WithField(accessibility, typeof(T).CSharpName(), name);

        public Class WithUsing(string usingName)
        {
            _usings.Add(usingName);
            return this;
        }

        public Class WithUsings(params string[] usings)
        {
            foreach (string? item in usings)
            {
                _usings.Add(item);
            }

            return this;
        }

        public Class WithBaseClass(string className)
        {
            _baseClasses.Add(className);
            return this;
        }

        //public override int GetHashCode() => HashCode.Combine(Namespace, ClassName);
        public override int GetHashCode()
        {
            if (Namespace is { })
            {
                return Namespace.GetHashCode() ^ ClassName.GetHashCode();
            }

            return ClassName.GetHashCode();
        }

        public bool Equals(Class other) => Namespace?.Equals(other.Namespace) == true && ClassName.Equals(other.ClassName);
    }
}
