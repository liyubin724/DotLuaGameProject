using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotEngine.Core.Crypto
{
    public enum MD5Length
    {
        L16 = 16,
        L32 = 32
    }

    public static class MD5Crypto
    {
        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted.</param>
        /// <param name="length">The length of hash result , default value is <see cref="MD5Length.L32"/>.</param>
        /// <returns></returns>
        public static string Md5(string srcString, MD5Length length = MD5Length.L32)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(srcString);
            return Md5(bytes, length);
        }

        public static string Md5(byte[] bytes,MD5Length length = MD5Length.L32)
        {
            string str_md5_out = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_md5_out = md5.ComputeHash(bytes);

                str_md5_out = length == MD5Length.L32
                    ? BitConverter.ToString(bytes_md5_out)
                    : BitConverter.ToString(bytes_md5_out, 4, 8);

                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }
        /// <summary>
        /// 计算文件的MD值，如果文件读取失败将返回string.Empty
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Md5File(string filePath, MD5Length length = MD5Length.L32)
        {
            if(!File.Exists(filePath))
            {
                return string.Empty;
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            if(bytes==null || bytes.Length == 0)
            {
                return string.Empty;
            }
            return Md5(bytes, length);
        }

        /// <summary>
        /// HMACMD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACMD5(string srcString, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(srcString);
            return HMACMD5(bytes, key);
        }

        public static string HMACMD5(byte[] bytes ,string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACMD5 md5 = new HMACMD5(secrectKey))
            {
                byte[] bytes_md5_out = md5.ComputeHash(bytes);
                string str_md5_out = BitConverter.ToString(bytes_md5_out);
                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }


    }
}
