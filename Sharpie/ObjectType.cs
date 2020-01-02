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
        public static string ToSharpieString(this ObjectType objectType) => objectType switch
        {
            ObjectType.Class => "class",
            ObjectType.Struct => "struct",
            _ => throw new ArgumentException(),
        };
    }
}
