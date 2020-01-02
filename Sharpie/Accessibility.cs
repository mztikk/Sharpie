using System;

namespace Sharpie
{
    public enum Accessibility
    {
        Public,
        Protected,
        Internal,
        ProtectedInternal,
        Private,
        PrivateProtected
    }

    public static class AccessibilityHelper
    {
        public static string ToSharpieString(this Accessibility accessibility) => accessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.ProtectedInternal => "protected internal",
            Accessibility.Private => "private",
            Accessibility.PrivateProtected => "private protected",
            _ => throw new ArgumentException(),
        };
    }
}
