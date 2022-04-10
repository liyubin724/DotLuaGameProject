using System;

namespace DotEngine.Utilities
{
    public static class StringUtility
    {
        private static string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
        /// <summary>
        /// 将字节数转换为其它单位
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String FormatByteSize(long size)
        {
            float result = size;
            int mod = 1024;
            int i = 0;
            while (result >= mod)
            {
                result /= mod;
                i++;
            }

            return $"{result.ToString("f2")}{units[i]}";
        }
    }
}
