using System;
using System.Reflection;
using System.Text;

namespace DotTool.Net
{
    public static class PBConfigLuaWriter
    {
        public static string WriteToLua(PBMessageConfig config)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"local {config.ClassName} = {{");

            int indent = 0;
            foreach(var group in config.Groups)
            {
                indent++;
                {
                    builder.AppendLine($"{GetIndent(indent)}--Begin {group.Name}--");
                    foreach(var message in group.Messages)
                    {
                        AppendMessage(builder, indent, message);
                    }
                    builder.AppendLine($"{GetIndent(indent)}--End {group.Name}--");
                    builder.AppendLine();
                }
                indent--;
            }

            builder.AppendLine("}");
            builder.AppendLine($"return {config.ClassName}");

            return builder.ToString();
        }

        private static void AppendMessage(StringBuilder builder,int indent,PBMessage message)
        {
            if(!string.IsNullOrEmpty(message.Comment))
            {
                string[] splitComments = message.Comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if(splitComments!=null && splitComments.Length>0)
                {
                    foreach(var comment in splitComments)
                    {
                        builder.AppendLine($"{GetIndent(indent)}--{comment}");
                    }
                }
            }
            builder.AppendLine($"{GetIndent(indent)}{message.Name} = {{");
            indent++;
            {
                PropertyInfo[] properties = typeof(PBMessage).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach(var property in properties)
                {
                    builder.Append($"{GetIndent(indent)}{property.Name} = ");
                    if(property.PropertyType == typeof(string))
                    {
                        builder.AppendLine($"[[{property.GetValue(message).ToString()}]],");
                    }
                    else if(property.PropertyType == typeof(bool))
                    {
                        builder.AppendLine($"{property.GetValue(message).ToString().ToLower()},");
                    }
                    else
                    {
                        builder.AppendLine($"{property.GetValue(message).ToString()},");
                    }
                }
            }
            indent--;
            builder.AppendLine($"{GetIndent(indent)}}},");
        }

        private static string GetIndent(int indent)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < indent * 4; i++)
            {
                builder.Append(" ");
            }
            return builder.ToString();
        }
    }
}
