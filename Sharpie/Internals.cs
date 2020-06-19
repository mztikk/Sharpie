using System;
using System.Collections.Generic;
using RFReborn;
using RFReborn.Extensions;
using RFReborn.RandomR;

namespace Sharpie
{
    internal static class Internals
    {
        internal static Random s_random = new CryptoRandom();
        internal const string Alphabet = StringR.AlphabetUpper + StringR.AlphabetLower;

        private static readonly HashSet<string> s_usedNames = new HashSet<string>();
        private static readonly object s_usedNamesLocker = new object();

        internal static string GetIdentifierName(int len = 8)
        {
            while (true)
            {
                string gen = s_random.NextString(Alphabet, len);

                lock (s_usedNamesLocker)
                {
                    if (s_usedNames.Contains(gen))
                    {
                        continue;
                    }

                    if (s_usedNames.Add(gen))
                    {
                        return gen;
                    }
                }
            }
        }
    }
}
