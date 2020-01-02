using System.Collections.Generic;
using System.IO;

namespace Sharpie
{
    public static class StringHelper
    {
        public static IEnumerable<string> GetLines(this string s)
        {
            using (StringReader reader = new StringReader(s))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line is null)
                    {
                        break;
                    }

                    yield return line;
                }
            }
        }
    }
}
