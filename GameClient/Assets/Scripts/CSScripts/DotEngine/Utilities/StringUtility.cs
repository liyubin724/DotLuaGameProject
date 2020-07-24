using System;
using System.Text.RegularExpressions;

namespace DotEngine.Utilities
{
    public static class StringUtility
    {
        /// <summary>
        /// 替换部分特殊字符为指定的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceSpecialCharacter(this string input, string replacement)
        {
            return Regex.Replace(input, "[ \\[ \\] \\^ \\-_*×――(^)|'$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", replacement);
        }

        /// <summary>
        /// 将字符串的特殊字符替换为文字表示形式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EscapeSpecialCharacterToLiteral(this string input)
        {
            return input.Replace("\\", @"\\")
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
        }

        /// <summary>
        /// 将字符串按行进行切分，并删除所有的空行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] SplitToNotEmptyLines(this string value)
        {
            return value.Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 将字符串按行进行切分，保留空行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] SplitToLines(this string value)
        {
            return value.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
        }

        /// <summary>
        /// 将字符串按指定的字符进行切分，保留空白
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string[] SplitToArray(this string value,char splitChar)
        {
            return value.Split(new char[] { splitChar }, StringSplitOptions.None);
        }

        /// <summary>
        /// 将字符串按指定的字符进行切分，删除空白
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string[] SplitToNotEmptyArray(this string value, char splitChar)
        {
            return value.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 计算字符串的time33的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public unsafe static uint GetTime33(this string value)
        {
            uint hash = 5381;
            fixed (char* str = value)
            {
                int index = 0;
                while (*(str + index) != 0)
                {
                    hash += (hash << 5) + *(str + index);
                    index++;
                }
            }
            return (hash & 0x7FFFFFFF);
        }
    }
}
