using System;
using System.Collections.Generic;
using System.IO;
using RFReborn.Extensions;
using Sharpie.Writer;

namespace Sharpie
{
    public static class StringHelper
    {
        public static IEnumerable<string> GetLines(this string s)
        {
            using (StringReader reader = new StringReader(s))
            {
                string line;
                while ((line = reader.ReadLine()) is { })
                {
                    yield return line;
                }
            }
        }

        public static Action<IndentedStreamWriter> StringToCall(string s) => (IndentedStreamWriter writer) => s.GetLines().Call(writer.WriteLine);
    }
}
