/*The file was created by tool.
-----------------------------------------------
Please don't change it manually!!!
Please don't change it manually!!!
Please don't change it manually!!!
-----------------------------------------------
*/
using DotEngine.Net.Message;
using DotEngine.Crypto;
using SnappySharp = Snappy.Sharp.Snappy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Net.Proto
{
    public class ClientMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }
        public string SecretVector{get;set;}

        private Dictionary<int,Func<object,byte[]>> encodeParserDic = new Dictionary<int,Func<object,byte[]>>();
        private Dictionary<int,Func<byte[],object>> decodeParserDic = new Dictionary<int,Func<byte[],object>>();

        public ClientMessageParser()
        {
            encodeParserDic.Add(10002,ShopListRequestEncodeParser);            encodeParserDic.Add(10001,LoginRequestEncodeParser);

            decodeParserDic.Add(1,LoginResponseDecodeParser);
            decodeParserDic.Add(2,ShopListResponseDecodeParser);
        }

        public byte[] EncodeMessage(int messageID, object message)
        {
            if(encodeParserDic.TryGetValue(messageID,out Func<object,byte[]> parser))
            {
                return parser(message);
            }
            return null;
        }

        public object DecodeMessage(int messageID, byte[] bytes)
        {
            if(decodeParserDic.TryGetValue(messageID,out Func<byte[],object> parser))
            {
                return parser(bytes);
            }
            return null;
        }


        //获取商店物品列表
        private byte[] ShopListRequestEncodeParser(object message)
        {
            string jsonStr = JsonConvert.SerializeObject(message, Formatting.None);
            byte[] messageBytes = Encoding.UTF8.GetBytes(jsonStr);

            messageBytes = AESCrypto.Encrypt(messageBytes, SecretKey, SecretVector);

            messageBytes = SnappySharp.Compress(messageBytes);

            return messageBytes;
        }

        //登录信息
        private byte[] LoginRequestEncodeParser(object message)
        {
            string jsonStr = JsonConvert.SerializeObject(message, Formatting.None);
            byte[] messageBytes = Encoding.UTF8.GetBytes(jsonStr);

            messageBytes = AESCrypto.Encrypt(messageBytes, SecretKey, SecretVector);

            messageBytes = SnappySharp.Compress(messageBytes);

            return messageBytes;
        }


        //返回登录结果
        private object LoginResponseDecodeParser(byte[] bytes)
        {
            byte[] messageBytes = bytes;

            messageBytes = SnappySharp.Uncompress(messageBytes);

            messageBytes = AESCrypto.Decrypt(messageBytes, SecretKey, SecretVector);

            string jsonStr = Encoding.UTF8.GetString(messageBytes);
            return JsonConvert.DeserializeObject(jsonStr,typeof(LoginResponse));
        }

        //返回商店物品列表
        private object ShopListResponseDecodeParser(byte[] bytes)
        {
            byte[] messageBytes = bytes;

            messageBytes = SnappySharp.Uncompress(messageBytes);

            messageBytes = AESCrypto.Decrypt(messageBytes, SecretKey, SecretVector);

            string jsonStr = Encoding.UTF8.GetString(messageBytes);
            return JsonConvert.DeserializeObject(jsonStr,typeof(ShopListResponse));
        }

    }
}