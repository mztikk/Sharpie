using System;

namespace Sharpie
{
    public static class TypeHelper
    {
        public static string CSharpName(this Type t) => t.Name switch
        {
            "SByte" => "sbyte",
            "Byte" => "byte",
            "Int16" => "short",
            "UInt16" => "ushort",
            "Int32" => "int",
            "UInt32" => "uint",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Single" => "float",
            "Double" => "double",
            "Boolean" => "bool",
            "Char" => "char",
            "String" => "string",
            "Object" => "object",
            _ => t.Name,
        };
    }
}
