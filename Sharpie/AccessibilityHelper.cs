using System;
using Microsoft.CodeAnalysis;

namespace Sharpie
{
    public static class AccessibilityHelper
    {
        public static string ToSharpieString(this Accessibility accessibility) => accessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.ProtectedAndInternal => "protected internal",
            Accessibility.Private => "private",
            _ => throw new ArgumentException(),
        };
    }
}
