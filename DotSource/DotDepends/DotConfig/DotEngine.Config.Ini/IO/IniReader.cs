using System;
using System.Linq;
using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    public static class IniReader
    {
        private static IniSchemeStyle schemeStyle = null;
        private static IniReaderStyle readerStyle = null;
        private static IniText textBuffer = new IniText();

        private static string cachedSectionName = null;
        private static List<string> cachedComments = new List<string>();
        private static List<string> cachedOptionalValues = new List<string>();

        public static IniConfig ReadFromString(string iniString, IniSchemeStyle schemeStyle = null, IniReaderStyle readerStyle = null)
        {
            if(string.IsNullOrEmpty(iniString))
            {
                return null;
            }

            IniReader.schemeStyle = schemeStyle ?? new IniSchemeStyle();
            IniReader.readerStyle = readerStyle ?? new IniReaderStyle() ;
            cachedComments.Clear();
            cachedOptionalValues.Clear();
            cachedSectionName = null;

            textBuffer.Text = iniString;
            IniConfig iniConfig = new IniConfig();
            IniLine iniLine = new IniLine();

            int lineStartIndex = 0;
            while(textBuffer.ReadLine(lineStartIndex,ref iniLine))
            {
                lineStartIndex = iniLine.End + 1;
                if (textBuffer.IsContentWhitespace(iniLine))
                {
                    continue;
                }
                if (ProcessCommentLine(iniLine))
                {
                    continue;
                }
                if(ProcessSection(iniLine,out var section))
                {
                    iniConfig.AddSection(section);
                    section.Comments.AddRange(cachedComments);
                    cachedComments.Clear();
                    continue;
                }
                if(ProcessOptionalValue(iniLine))
                {
                    continue;
                }
                if(ProcessProperty(iniLine,out var property))
                {
                    property.Comments.AddRange(cachedComments);
                    property.OptionalValues.AddRange(cachedOptionalValues);

                    var s = iniConfig.GetSection(cachedSectionName);
                    s.AddProperty(property);
                    continue;
                }
            }

            return iniConfig;
        }

        private static bool ProcessSection(IniLine line,out IniSection section)
        {
            section = null;
            if (!textBuffer.IsContentStartWith(line, schemeStyle.SectionPrefix))
            {
                return false;
            }
            int index = textBuffer.FindContent(line.ContentStart, line.ContentEnd, schemeStyle.OptionalValuePostfix);
            if (index < 0)
            {
                throw new IniReaderLineFormatException(textBuffer.GetContent(line));
            }
            int prefixLen = schemeStyle.SectionPrefix.Length;
            string sectionName = textBuffer.GetContent(line.ContentStart + prefixLen, index);
            if(string.IsNullOrEmpty(sectionName))
            {
                throw new IniReaderLineFormatException(textBuffer.GetContent(line));
            }
            if(string.IsNullOrEmpty(cachedSectionName) || cachedSectionName!=sectionName)
            {
                section = new IniSection(sectionName);
            }
            cachedSectionName = sectionName;
            return true;
        }

        private static bool ProcessCommentLine(IniLine line)
        {
            if(!textBuffer.IsContentStartWith(line,schemeStyle.CommentPrefix))
            {
                return false;
            }
            if(!readerStyle.IsParseComments)
            {
                return true;
            }
            int commentPrefixLen = schemeStyle.CommentPrefix.Length;
            string comment = textBuffer.GetContent(line.ContentStart + commentPrefixLen, line.ContentSize);
            if(readerStyle.IsTrimComments)
            {
                comment = comment.Trim();
            }
            if(!string.IsNullOrEmpty(comment))
            {
                cachedComments.Add(comment);
            }
            return true;
        }

        private static bool ProcessOptionalValue(IniLine line)
        {
            if (!textBuffer.IsContentStartWith(line, schemeStyle.OptionalValuePrefix))
            {
                return false;
            }
            int index = textBuffer.FindContent(line.ContentStart, line.ContentEnd, schemeStyle.OptionalValuePostfix);
            if(index < 0)
            {
                throw new IniReaderLineFormatException(textBuffer.GetContent(line));
            }
            if(!readerStyle.IsParseOptionalValues)
            {
                return true;
            }

            int prefixLen = schemeStyle.OptionalValuePostfix.Length;
            string valueContent = textBuffer.GetContent(line.ContentStart + prefixLen, index);
            string[] values = valueContent.Split(new string[] { schemeStyle.OptionalValueAssigment }, StringSplitOptions.RemoveEmptyEntries);
            if(values!=null && values.Length>0)
            {
                if(readerStyle.IsTrimOptionalValues)
                {
                    values = (from value in values let v = value.Trim() where !string.IsNullOrEmpty(v) select v).ToArray();
                }
                if(values.Length>0)
                {
                    cachedOptionalValues.AddRange(values);
                }
            }
            return true;
        }

        private static bool ProcessProperty(IniLine line,out IniProperty property)
        {
            property = null;
            string valueContent = textBuffer.GetContent(line);
            if(string.IsNullOrEmpty(valueContent))
            {
                return false;
            }
            if(string.IsNullOrEmpty(cachedSectionName))
            {
                throw new IniReaderLineSectionNullException();
            }

            int assigmentIndex = valueContent.IndexOf(schemeStyle.PropertyAssigment);
            if(assigmentIndex<0)
            {
                throw new IniReaderLineFormatException(valueContent);
            }
            string propertyKey = valueContent.Substring(line.ContentStart, assigmentIndex);
            string propertyValue = valueContent.Substring(assigmentIndex + schemeStyle.PropertyAssigment.Length, line.ContentEnd);
            if(readerStyle.IsTrimPropertyKey)
            {
                propertyKey = propertyKey.Trim();
            }
            if(string.IsNullOrEmpty(propertyKey))
            {
                throw new IniReaderLinePropertyKeyEmptyException();
            }
            if(readerStyle.IsTrimPropertyValue)
            {
                propertyValue = propertyValue.Trim();
            }

            property = new IniProperty(propertyKey, propertyValue);

            return true;
        }
    }
}
