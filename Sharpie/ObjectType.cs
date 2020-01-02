using System;

namespace Sharpie
{
    public enum ObjectType
    {
        Class,
        Struct
    }

    public static class ObjectTypeHelper
    {
        public static string ToSharpieString(this ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Class:
                    return "class";
                case ObjectType.Struct:
                    return "struct";
                default:
                    throw new ArgumentException();
            }
        }
    }
}
