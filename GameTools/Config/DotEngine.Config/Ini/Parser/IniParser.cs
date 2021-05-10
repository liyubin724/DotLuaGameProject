using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.Ini
{
    public class IniParser
    {
        private static IniSchemeStyle schemeStyle = null;
        private static IniParserStyle parserStyle = null;
        private static List<string> tempComments = new List<string>();
        private static List<Exception> tempExceptions = new List<Exception>();

        public static bool Parse(string iniString, out IniData iniData, IniSchemeStyle scheme = null, IniParserStyle parser = null)
        {
            schemeStyle = scheme;
            parserStyle = parser;

            iniData = new IniData();

            IniStringBuffer stringBuffer = new IniStringBuffer(iniString);
            IniLineRange lineRange = new IniLineRange();
            if (stringBuffer.ReadLine(ref lineRange))
            {
                try
                {
                    ProcessLine(stringBuffer, lineRange, iniData);
                }catch(Exception e)
                {
                    tempExceptions.Add(e);
                    if(parser.ThrowExceptionsOnError)
                    {
                        throw;
                    }
                }
            }

            return true;
        }

        private static void ProcessLine(IniStringBuffer buffer, IniLineRange lineRange, IniData iniData)
        {
            if (buffer.IsEmpty(lineRange) || buffer.IsWhitespace(lineRange)) return;

            if (ProcessComment(buffer, lineRange, iniData)) return;
            if (ProcessOptionalValue(buffer, lineRange, iniData)) return;
            if (ProcessSection(buffer, lineRange, iniData)) return;
            if (ProcessProperty(buffer, lineRange, iniData)) return;


        }

        private static bool ProcessComment(IniStringBuffer buffer, IniLineRange lineRange, IniData iniData)
        {
            IniLineRange range = lineRange.DeepCopy();
            buffer.TrimStart(range);
            if (!buffer.IsStartWith(range, schemeStyle.CommentString))
            {
                return false;
            }
            if (!parserStyle.IsParseComments)
            {
                return true;
            }
            int commentStartIndex = buffer.FindString(range, schemeStyle.CommentString);

            range.Start = commentStartIndex + schemeStyle.CommentString.Length;
            if (parserStyle.IsTrimComments)
            {
                buffer.Trim(range);
            }
            string comment = buffer.GetString(range);
            tempComments.Add(comment);
            return true;
        }

        private static bool ProcessSection(IniStringBuffer buffer, IniLineRange lineRange, IniData iniData)
        {
            IniLineRange range = lineRange.DeepCopy();
            buffer.Trim(range);
            if (!buffer.IsStartWith(range, schemeStyle.SectionStartString))
            {
                return false;
            }
            if (!buffer.IsEndWith(range, schemeStyle.SectionEndString))
            {

                return false;
            }

            int startIndex = range.Start + schemeStyle.SectionStartString.Length;
            int endIndex = range.End - schemeStyle.SectionEndString.Length;
            range.Start = startIndex;
            range.Size = endIndex - startIndex + 1;

            if (parserStyle.IsTrimSections)
            {
                buffer.Trim(range);
            }

            string sectionName = buffer.GetString(range);

        }

        private static bool ProcessOptionalValue(IniStringBuffer buffer, IniLineRange lineRange, IniData iniData)
        {

        }

        private static bool ProcessProperty(IniStringBuffer buffer, IniLineRange lineRange, IniData iniData)
        {

        }
    }
}
