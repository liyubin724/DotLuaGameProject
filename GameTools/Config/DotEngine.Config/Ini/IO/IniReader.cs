using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.Ini
{
    public class IniReader
    {
        private static IniSchemeStyle schemeStyle = null;
        private static IniReaderStyle readerStyle = null;
        private static List<string> tempComments = new List<string>();
        private static List<string> tempOptionalValues = new List<string>();
        private static string tempSectionName = null;
        private static List<Exception> tempExceptions = new List<Exception>();

        public static bool ReadFromString(string iniString, out IniData iniData, IniSchemeStyle scheme = null, IniReaderStyle parser = null)
        {
            schemeStyle = scheme??new IniSchemeStyle();
            readerStyle = parser??new IniReaderStyle();
            tempComments.Clear();
            tempOptionalValues.Clear();
            tempSectionName = null;
            tempExceptions.Clear();
            
            iniData = new IniData();

            IniTextBuffer stringBuffer = new IniTextBuffer(iniString);
            while (stringBuffer.ReadLine())
            {
                try
                {
                    ProcessLine(stringBuffer, iniData);
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

        private static void ProcessLine(IniTextBuffer buffer, IniData iniData)
        {
            if (buffer.IsEmpty(buffer.Range) || buffer.IsWhitespace(buffer.Range)) return;

            if (ProcessComment(buffer, iniData)) return;
            if (ProcessOptionalValue(buffer, iniData)) return;
            if (ProcessSection(buffer, iniData)) return;
            if (ProcessProperty(buffer, iniData)) return;


        }

        private static bool ProcessComment(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.TrimStart(range);
            if (!buffer.IsStartWith(range, schemeStyle.CommentString))
            {
                return false;
            }
            if (!readerStyle.IsParseComments)
            {
                return true;
            }

            int startIndex = buffer.FindString(range, schemeStyle.CommentString) + schemeStyle.CommentString.Length;
            int endIndex = range.End;
            range.Start = startIndex;
            range.Size = endIndex - startIndex + 1;

            if (readerStyle.IsTrimComments)
            {
                buffer.Trim(range);
            }

            string comment = buffer.GetString(range);
            tempComments.Add(comment);

            return true;
        }

        private static bool ProcessSection(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.Trim(range);
            if (!buffer.IsStartWith(range, schemeStyle.SectionStartString))
            {
                return false;
            }
            if (!buffer.IsEndWith(range, schemeStyle.SectionEndString))
            {
                if(!readerStyle.ThrowExceptionsOnError)
                {
                    return false;
                }
                throw new IniReaderException($"Error:No closing section value({schemeStyle.SectionEndString}). ", buffer.LineNumber, buffer.LineContent);
            }

            int startIndex = range.Start + schemeStyle.SectionStartString.Length;
            int endIndex = range.End - schemeStyle.SectionEndString.Length;
            range.Start = startIndex;
            range.Size = endIndex - startIndex + 1;

            if (readerStyle.IsTrimSections)
            {
                buffer.Trim(range);
            }

            string sectionName = buffer.GetString(range);
            if(string.IsNullOrEmpty(sectionName))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return false;
                }
                throw new IniReaderException($"Error:The name of section is empty. ", buffer.LineNumber, buffer.LineContent);
            }

            tempSectionName = sectionName;
            Section section = iniData.AddSection(tempSectionName);
            if(readerStyle.IsParseComments && tempComments.Count>0)
            {
                section.Comments = tempComments;
                tempComments.Clear();
            }
            return true;
        }

        private static bool ProcessOptionalValue(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.Trim(range);
            if (!buffer.IsStartWith(range, schemeStyle.OptionalValueStartString))
            {
                return false;
            }
            if (!buffer.IsEndWith(range, schemeStyle.OptionalValueEndString))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return false;
                }
                throw new IniReaderException($"Error:No closing option value({schemeStyle.OptionalValueEndString}). ", buffer.LineNumber, buffer.LineContent);
            }

            if(!readerStyle.IsParseOptionalValues)
            {
                return true;
            }

            int startIndex = range.Start + schemeStyle.SectionStartString.Length;
            int endIndex = range.End - schemeStyle.SectionEndString.Length;
            range.Start = startIndex;
            range.Size = endIndex - startIndex + 1;

            string optionalValueStr = buffer.GetString(range);
            if(string.IsNullOrEmpty(optionalValueStr))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return false;
                }
                throw new IniReaderException($"Error:The value of optionalValue is empty. ", buffer.LineNumber, buffer.LineContent);
            }

            string[] optionalValues = optionalValueStr.Split(new string[] { schemeStyle.OptionalValueAssigmentString }, StringSplitOptions.RemoveEmptyEntries);
            if (readerStyle.IsTrimOptionalValues)
            {
                foreach(var v in optionalValues)
                {
                    string trimedValue = v.Trim();
                    if(!string.IsNullOrEmpty(trimedValue))
                    {
                        tempOptionalValues.Add(trimedValue);
                    }
                }
            }else
            {
                tempOptionalValues.AddRange(optionalValues);
            }
            return true;
        }

        private static bool ProcessProperty(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.TrimStart(range);

            int assigmentStartIndex = buffer.FindString(range, schemeStyle.PropertyAssigmentString);
            if(assigmentStartIndex<0)
            {

                return false;
            }

            IniLineRange keyRange = new IniLineRange();
            keyRange.Start = range.Start;
            keyRange.Size = assigmentStartIndex - range.Start;
            buffer.Trim(keyRange);
            if(buffer.IsEmpty(keyRange))
            {
                return false;
            }
            string propertyKey = buffer.GetString(keyRange);

            IniLineRange valueRange = new IniLineRange();
            valueRange.Start = assigmentStartIndex + schemeStyle.PropertyAssigmentString.Length;
            valueRange.Size = range.End - valueRange.Start + 1;
            if(readerStyle.IsTrimProperties)
            {
                buffer.Trim(valueRange);
            }
            string propertyValue = buffer.GetString(valueRange);

            if(string.IsNullOrEmpty(tempSectionName))
            {
                tempSectionName = IniData.GLOBAL_SECTION_NAME;
            }
            Section section = iniData.GetSection(tempSectionName, true);
            if(section.ContainsProperty(propertyKey))
            {
                return false;
            }
            Property property = section.AddProperty(propertyKey, propertyValue);
            if(readerStyle.IsParseComments && tempComments.Count>0)
            {
                property.Comments = tempComments;
                tempComments.Clear();
            }
            if(readerStyle.IsParseOptionalValues && tempOptionalValues.Count > 0)
            {
                property.OptionalValues = tempOptionalValues;
                tempOptionalValues.Clear();
            }
            return true;
        }
    }
}
