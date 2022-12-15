using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2022.Solutions
{
    public static class InputExtensions
    {
        public static string[] SplitByNewlines(this string s)
        {
            return s
                .Replace("\r\n", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitByDoubleNewlines(this string s, char replaceDoubleNewline = '*')
        {
            return s
                .Replace("\r\n", "\n")
                .Replace("\n\n", replaceDoubleNewline.ToString())
                .Split(replaceDoubleNewline, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] Split(this string s, string splitBy, char replacement = '*')
        {
            return s
                .Replace(splitBy, replacement.ToString())
                .Split(replacement, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
