using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotTools.CodeGen
{
    class TemplateFormatException : Exception
    {
        public TemplateFormatException(string message)
        {
        }
    }

    public static class Scanner
    {
        private static string regexBadUnopened = @"(?<error>((?!<%).)*%>)";
        private static string regexText = @"(?<text>((?!<%).)+)";
        private static string regexNoCode = @"(?<nocode><%=?%>)";
        private static string regexCode = @"<%(?<code>[^=]((?!<%|%>).)*)%>";
        private static string regexEval = @"<%=(?<eval>((?!<%|%>).)*)%>";
        private static string regexBadUnclosed = @"(?<error><%.*)";
        private static string regexBadEmpty = @"(?<error>^$)";

        private static string GetRegexPattern()
        {
            return '(' + regexBadUnopened
                + '|' + regexText
                + '|' + regexNoCode
                + '|' + regexCode
                + '|' + regexEval
                + '|' + regexBadUnclosed
                + '|' + regexBadEmpty
                + ")*";
        }

        public static List<Chunk> Parse(string snippet)
        {
            Regex templateRegex = new Regex(
                GetRegexPattern(),
                RegexOptions.ExplicitCapture | RegexOptions.Singleline
            );
            Match matches = templateRegex.Match(snippet);

            if (matches.Groups["error"].Length > 0)
            {
                throw new TemplateFormatException("Messed up brackets");
            }

            List<Chunk> Chunks = matches.Groups["code"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Code, p.Value, p.Index })
                .Concat(matches.Groups["text"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Text, Value = EscapeString(p.Value), p.Index }))
                .Concat(matches.Groups["eval"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Eval, p.Value, p.Index }))
                .OrderBy(p => p.Index)
                .Select(m => new Chunk(m.Type, m.Value))
                .ToList();

            if (Chunks.Count == 0)
            {
                throw new TemplateFormatException("Empty template");
            }
            return Chunks;
        }

        private static string EscapeString(string input)
        {
            var output = input
                .Replace("\\", @"\\")
                .Replace("\'", @"\'")
                .Replace("\"", @"\""")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace("\r", @"\r")
                .Replace("\b", @"\b")
                .Replace("\f", @"\f")
                .Replace("\a", @"\a")
                .Replace("\v", @"\v")
                .Replace("\0", @"\0");
            return output;
        }
    }
}
