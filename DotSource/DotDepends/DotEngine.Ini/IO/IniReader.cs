using System;
using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    enum ProcessLineState
    {
        Success = 0,
        Unrecognized = 1,
        Error = 2,
    }

    public class IniReader
    {
        private static IniSchemeStyle schemeStyle = null;

        private static IniReaderStyle readerStyle = null;
        private static List<string> tempComments = new List<string>();
        private static List<string> tempOptionalValues = new List<string>();
        private static string tempSectionName = null;
        private static List<Exception> tempExceptions = new List<Exception>();

        public static Exception[] GetReaderExceptions
        {
            get
            {
                return tempExceptions.ToArray();
            }
        }

        public static IniData ReadFromString(string iniString,  IniSchemeStyle schemeStyle = null, IniReaderStyle readerStyle = null)
        {
            IniReader.schemeStyle = schemeStyle;
            IniReader.readerStyle = readerStyle;

            BeginRead();

            IniData iniData = new IniData();

            IniTextBuffer stringBuffer = new IniTextBuffer(iniString);
            while (stringBuffer.ReadLine())
            {
                try
                {
                    ProcessLine(stringBuffer, iniData);
                }
                catch (Exception e)
                {
                    tempExceptions.Add(e);

                    if (readerStyle.ThrowExceptionsOnError)
                    {
                        EndRead();

                        throw;
                    }
                }
            }

            if(tempExceptions.Count>0)
            {
                iniData = null;
            }

            EndRead();
            
            return iniData;
        }

        private static void BeginRead()
        {
            if(schemeStyle == null)
            {
                schemeStyle = new IniSchemeStyle();
            }
            if(readerStyle == null)
            {
                readerStyle = new IniReaderStyle();
            }
            tempComments.Clear();
            tempOptionalValues.Clear();
            tempSectionName = null;
            tempExceptions.Clear();
        }

        private static void EndRead()
        {
            schemeStyle = null;
            readerStyle = null;
            tempComments.Clear();
            tempOptionalValues.Clear();
            tempSectionName = null;
            tempExceptions.Clear();
        }

        private static void ProcessLine(IniTextBuffer buffer, IniData iniData)
        {
            if (buffer.IsEmpty(buffer.Range) || buffer.IsWhitespace(buffer.Range)) return;

            ProcessLineState state = ProcessComment(buffer, iniData);
            if (state == ProcessLineState.Error || state == ProcessLineState.Success) return;
            state = ProcessOptionalValue(buffer, iniData);
            if (state == ProcessLineState.Error || state == ProcessLineState.Success) return;
            state = ProcessSection(buffer, iniData);
            if (state == ProcessLineState.Error || state == ProcessLineState.Success) return;
            state = ProcessProperty(buffer, iniData);
            if (state == ProcessLineState.Error || state == ProcessLineState.Success) return;

            throw new IniReaderException("Error:Couldn't parse text", buffer.LineNumber, buffer.LineContent);
        }

        private static ProcessLineState ProcessComment(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.TrimStart(range);
            if (!buffer.IsStartWith(range, schemeStyle.CommentString))
            {
                return ProcessLineState.Unrecognized;
            }
            if (!readerStyle.IsParseComments)
            {
                return ProcessLineState.Success;
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

            return ProcessLineState.Success;
        }

        private static ProcessLineState ProcessSection(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.Trim(range);
            if (!buffer.IsStartWith(range, schemeStyle.SectionStartString))
            {
                return ProcessLineState.Unrecognized;
            }
            if (!buffer.IsEndWith(range, schemeStyle.SectionEndString))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
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
            if (string.IsNullOrEmpty(sectionName))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
                }
                throw new IniReaderException($"Error:The name of section is empty. ", buffer.LineNumber, buffer.LineContent);
            }

            tempSectionName = sectionName;
            IniSection section = iniData.AddSection(tempSectionName);
            if (readerStyle.IsParseComments && tempComments.Count > 0)
            {
                section.Comments = tempComments;
                tempComments.Clear();
            }
            return ProcessLineState.Success;
        }

        private static ProcessLineState ProcessOptionalValue(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.Trim(range);
            if (!buffer.IsStartWith(range, schemeStyle.OptionalValueStartString))
            {
                return ProcessLineState.Unrecognized;
            }
            if (!buffer.IsEndWith(range, schemeStyle.OptionalValueEndString))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
                }
                throw new IniReaderException($"Error:No closing option value({schemeStyle.OptionalValueEndString}). ", buffer.LineNumber, buffer.LineContent);
            }

            if (!readerStyle.IsParseOptionalValues)
            {
                return ProcessLineState.Success;
            }

            int startIndex = range.Start + schemeStyle.OptionalValueStartString.Length;
            int endIndex = range.End - schemeStyle.OptionalValueEndString.Length;
            range.Start = startIndex;
            range.Size = endIndex - startIndex + 1;

            string optionalValueStr = buffer.GetString(range);
            if (string.IsNullOrEmpty(optionalValueStr))
            {
                if (!readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
                }
                throw new IniReaderException($"Error:The value of optionalValue is empty. ", buffer.LineNumber, buffer.LineContent);
            }

            string[] optionalValues = optionalValueStr.Split(new string[] { schemeStyle.OptionalValueAssigmentString }, StringSplitOptions.RemoveEmptyEntries);
            if (readerStyle.IsTrimOptionalValues)
            {
                foreach (var v in optionalValues)
                {
                    string trimedValue = v.Trim();
                    if (!string.IsNullOrEmpty(trimedValue))
                    {
                        tempOptionalValues.Add(trimedValue);
                    }
                }
            }
            else
            {
                tempOptionalValues.AddRange(optionalValues);
            }
            return ProcessLineState.Success;
        }

        private static ProcessLineState ProcessProperty(IniTextBuffer buffer, IniData iniData)
        {
            IniLineRange range = buffer.Range.DeepCopy();
            buffer.TrimStart(range);

            int assigmentStartIndex = buffer.FindString(range, schemeStyle.PropertyAssigmentString);
            if (assigmentStartIndex < 0)
            {
                return ProcessLineState.Unrecognized;
            }

            if (string.IsNullOrEmpty(tempSectionName))
            {
                if (readerStyle.AllowKeysWithoutSection)
                {
                    tempSectionName = IniData.GLOBAL_SECTION_NAME;
                }
                else
                {
                    if (!readerStyle.ThrowExceptionsOnError)
                    {
                        return ProcessLineState.Error;
                    }

                    throw new IniReaderException("Error:The section is not found for property", buffer.LineNumber, buffer.LineContent);
                }
            }

            IniLineRange keyRange = new IniLineRange();
            keyRange.Start = range.Start;
            keyRange.Size = assigmentStartIndex - range.Start;
            buffer.Trim(keyRange);
            if (buffer.IsEmpty(keyRange))
            {
                if (readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
                }
                throw new IniReaderException("Error:The key of property is empty.", buffer.LineNumber, buffer.LineContent);
            }

            IniLineRange valueRange = new IniLineRange();
            valueRange.Start = assigmentStartIndex + schemeStyle.PropertyAssigmentString.Length;
            valueRange.Size = range.End - valueRange.Start + 1;
            if (readerStyle.IsTrimProperties)
            {
                buffer.Trim(valueRange);
            }

            string propertyKey = buffer.GetString(keyRange);

            IniSection section = iniData.GetSection(tempSectionName, true);
            if (section.ContainsProperty(propertyKey))
            {
                if (readerStyle.ThrowExceptionsOnError)
                {
                    return ProcessLineState.Error;
                }
                throw new IniReaderException("Error:The key of property is repeated.", buffer.LineNumber, buffer.LineContent);
            }

            string propertyValue = buffer.GetString(valueRange);

            IniProperty property = section.AddProperty(propertyKey, propertyValue);
            if (readerStyle.IsParseComments && tempComments.Count > 0)
            {
                property.Comments = tempComments;
                tempComments.Clear();
            }
            if (readerStyle.IsParseOptionalValues && tempOptionalValues.Count > 0)
            {
                property.OptionalValues = tempOptionalValues;
                tempOptionalValues.Clear();
            }
            return ProcessLineState.Success;
        }
    }
}
