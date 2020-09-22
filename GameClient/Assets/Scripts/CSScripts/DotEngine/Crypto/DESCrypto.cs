﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable 0168
namespace DotEngine.Crypto
{
    public static class DESCrypto
    {
        /// <summary>
        /// Create des key
        /// </summary>
        /// <returns></returns>
        public static string CreateDesKey()
        {
            return CryptoHelper.GetRandomStr(24);
        }

        /// <summary>
        /// Create des iv
        /// </summary>
        /// <returns></returns>
        public static string CreateDesIv()
        {
            return CryptoHelper.GetRandomStr(8);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string DESEncrypt(string data, string key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            var encryptBytes = DESEncrypt(plainBytes, key, CipherMode.ECB);

            if (encryptBytes == null)
            {
                return null;
            }
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data byte array</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] DESEncrypt(byte[] data, string key)
        {
            return DESEncrypt(data, key, CipherMode.ECB);
        }


        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data byte array</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] DESEncrypt(byte[] data, string key, string vector)
        {
            return DESEncrypt(data, key, CipherMode.CBC, vector);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <param name="cipherMode"><see cref="CipherMode"/></param>  
        /// <param name="paddingMode"><see cref="PaddingMode"/> default is PKCS7</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        private static byte[] DESEncrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (MemoryStream Memory = new MemoryStream())
            {
                using (TripleDES des = TripleDES.Create())
                {
                    byte[] plainBytes = data;
                    byte[] bKey = new byte[24];
                    Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                    des.Mode = cipherMode;
                    des.Padding = paddingMode;
                    des.Key = bKey;

                    if (cipherMode == CipherMode.CBC)
                    {
                        byte[] bVector = new byte[8];
                        Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
                        des.IV = bVector;
                    }

                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return Memory.ToArray();
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                }
            }
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string DESDecrypt(string data, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(data);
            byte[] bytes = DESDecrypt(encryptedBytes, key, CipherMode.ECB);

            if (bytes == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data byte array</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static byte[] DESDecrypt(byte[] data, string key)
        {
            return DESDecrypt(data, key, CipherMode.ECB);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data byte array</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] DESDecrypt(byte[] data, string key, string vector)
        {
            return DESDecrypt(data, key, CipherMode.CBC, vector);
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <param name="cipherMode"><see cref="CipherMode"/></param>  
        /// <param name="paddingMode"><see cref="PaddingMode"/> default is PKCS7</param>  
        /// <returns>Decrypted byte array</returns>  
        private static byte[] DESDecrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            byte[] encryptedBytes = data;
            byte[] bKey = new byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            {
                using (TripleDES des = TripleDES.Create())
                {
                    des.Mode = cipherMode;
                    des.Padding = paddingMode;
                    des.Key = bKey;

                    if (cipherMode == CipherMode.CBC)
                    {
                        byte[] bVector = new byte[8];
                        Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
                        des.IV = bVector;
                    }

                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        try
                        {
                            byte[] tmp = new byte[encryptedBytes.Length];
                            int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                            byte[] ret = new byte[len];
                            Array.Copy(tmp, 0, ret, 0, len);
                            return ret;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
