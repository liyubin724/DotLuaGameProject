using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEngine.Config.Ini
{
    public static class IniWriter
    {
        private static IniSchemeStyle schemeStyle = null;
        private static IniWriterStyle writerStyle = null;

        public static string WriteToString(IniConfig iniData, IniSchemeStyle schemeStyle = null, IniWriterStyle writerStyle = null)
        {
            IniWriter.schemeStyle = schemeStyle ?? new IniSchemeStyle();
            IniWriter.writerStyle = writerStyle ?? new IniWriterStyle();

            TextWriter writer = new StringWriter(new StringBuilder());

            foreach (var section in iniData)
            {
                WriteSection(section, writer);
            }

            writer.Flush();
            
            string iniString = writer.ToString();
            writer.Close();

            return iniString;
        }

        private static void WriteSection(IniSection section, TextWriter writer)
        {
            if (writerStyle.IsNewLineBeforeSection)
            {
                writer.Write($"{writerStyle.NewLineString}");
            }

            List<string> comments = section.Comments;
            if (comments != null && comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    writer.Write($"{schemeStyle.CommentString}{comment}{writerStyle.NewLineString}");
                }
            }

            writer.Write($"{schemeStyle.SectionStartString}{section.Name}{schemeStyle.SectionEndString}{writerStyle.NewLineString}");

            if (writerStyle.IsNewLineAfterSection)
            {
                writer.Write($"{writerStyle.NewLineString}");
            }

            foreach (var property in section)
            {
                WriteProperty(property, writer);
            }
        }

        private static void WriteProperty(IniProperty property, TextWriter writer)
        {
            if (writerStyle.IsNewLineBeforeProperty)
            {
                writer.Write($"{writerStyle.NewLineString}");
            }

            List<string> comments = property.Comments;
            if (comments != null && comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    writer.Write($"{schemeStyle.CommentString}{comment}{writerStyle.NewLineString}");
                }
            }

            List<string> optionalValues = property.OptionalValues;
            if (optionalValues != null && optionalValues.Count > 0)
            {
                writer.Write($"{schemeStyle.OptionalValueStartString}");
                for (int i = 0; i < optionalValues.Count; ++i)
                {
                    writer.Write($"{optionalValues[i]}");
                    if (i < optionalValues.Count - 1)
                    {
                        writer.Write($"{schemeStyle.OptionalValueAssigmentString}");
                    }
                }
                writer.Write($"{schemeStyle.OptionalValueEndString}{writerStyle.NewLineString}");
            }

            writer.Write($"{property.Key}{writerStyle.SpacesBetweenKeyAndAssigment}{schemeStyle.PropertyAssigmentString}{writerStyle.SpacesBetweenAssigmentAndValue}{property.StringValue}{writerStyle.NewLineString}");
            if (writerStyle.IsNewLineAfterProperty)
            {
                writer.Write($"{writerStyle.NewLineString}");
            }
        }

    }
}
