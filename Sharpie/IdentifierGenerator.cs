//namespace Sharpie
//{
//    public static class IdentifierGenerator
//    {
//        private static readonly HashSet<string> s_usedNames = new HashSet<string>();
//        private static readonly object s_usedNamesLocker = new object();

//        public static string Get(int len = 8)
//        {
//            while (true)
//            {
//                string gen = Internals.s_random.NextString(Internals.Alphabet, len);

//                lock (s_usedNamesLocker)
//                {
//                    if (s_usedNames.Contains(gen))
//                    {
//                        continue;
//                    }

//                    if (s_usedNames.Add(gen))
//                    {
//                        return gen;
//                    }
//                }
//            }
//        }
//    }
//}
