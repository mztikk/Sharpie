using System;
using System.Collections.Concurrent;
using RFReborn;
using RFReborn.Extensions;
using RFReborn.RandomR;

namespace Sharpie
{
    internal static class Internals
    {
        internal const byte NULL_BYTE = 0x0;

        internal static Random s_random = new CryptoRandom();
        internal const string Alphabet = StringR.AlphabetUpper + StringR.AlphabetLower;

        private static readonly ConcurrentDictionary<string, byte> s_usedNames = new ConcurrentDictionary<string, byte>();

        internal static string GetIdentifierName(int len = 8)
        {
            while (true)
            {
                string gen = s_random.NextString(Alphabet, len);
                if (s_usedNames.ContainsKey(gen))
                {
                    continue;
                }

                if (s_usedNames.TryAdd(gen, NULL_BYTE))
                {
                    return gen;
                }
            }
        }
    }
}
