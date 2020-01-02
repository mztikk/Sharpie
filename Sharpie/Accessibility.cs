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
        public static string ToSharpieString(this Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Public:
                    return "public";
                case Accessibility.Protected:
                    return "protected";
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.ProtectedInternal:
                    return "protected internal";
                case Accessibility.Private:
                    return "private";
                case Accessibility.PrivateProtected:
                    return "private protected";
                default:
                    throw new ArgumentException();
            }
        }
    }
}
