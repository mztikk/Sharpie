﻿using System;
using System.Collections.Generic;
using System.IO;
using RFReborn.Extensions;
using Sharpie.Writer;

namespace Sharpie
{
    public static class StringHelper
    {
        /// <summary>
        /// Yields every line inside a string
        /// </summary>
        /// <param name="s">String to get lines from</param>
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

        /// <summary>
        /// Creates an Action that calls <see cref="IndentedStreamWriter.WriteLine(string)"/> on every line of <see langword="abstract"/>string
        /// </summary>
        /// <param name="s">String to turn into callable action</param>
        public static Action<IndentedStreamWriter> StringToCall(string s) => (IndentedStreamWriter writer) => s.GetLines().Call(writer.WriteLine);
    }
}
