/*The file was created by tool.
-----------------------------------------------
Please don't change it manually!!!
Please don't change it manually!!!
Please don't change it manually!!!
-----------------------------------------------
*/
using DotEngine.Net.Message;
using System;
using System.Collections.Generic;
using Google.Protobuf;
using DotEngine.Crypto;
using SnappySharp = Snappy.Sharp.Snappy;
using Game.Net.Protos;





namespace Game.Net.Proto
{
    public class ServerMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }
        public string SecretVector{get;set;}

        private Dictionary<int,Func<object,byte[]>> encodeParserDic = new Dictionary<int,Func<object,byte[]>>();
        private Dictionary<int,Func<byte[],object>> decodeParserDic = new Dictionary<int,Func<byte[],object>>();

        public ServerMessageParser()
        {

            encodeParserDic.Add(1,LoginResponseEncodeParser);
            encodeParserDic.Add(2,ShopListResponseEncodeParser);

            decodeParserDic.Add(10002,ShopListRequestDecodeParser);
            decodeParserDic.Add(10001,LoginRequestDecodeParser);
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


        private byte[] LoginResponseEncodeParser(object message)
        {
            IMessage m = (IMessage)message;
            byte[] messageBytes = m.ToByteArray();

            messageBytes = AESCrypto.Encrypt(messageBytes, SecretKey, SecretVector);


            messageBytes = SnappySharp.Compress(messageBytes);

            return messageBytes;
        }

        private byte[] ShopListResponseEncodeParser(object message)
        {
            IMessage m = (IMessage)message;
            byte[] messageBytes = m.ToByteArray();

            messageBytes = AESCrypto.Encrypt(messageBytes, SecretKey, SecretVector);


            messageBytes = SnappySharp.Compress(messageBytes);

            return messageBytes;
        }



        private object ShopListRequestDecodeParser(byte[] bytes)
        {
            byte[] messageBytes = bytes;

            messageBytes = SnappySharp.Uncompress(messageBytes);


            messageBytes = AESCrypto.Decrypt(messageBytes, SecretKey, SecretVector);

            return ShopListRequest.Parser.ParseFrom(messageBytes);
        }

        private object LoginRequestDecodeParser(byte[] bytes)
        {
            byte[] messageBytes = bytes;

            messageBytes = SnappySharp.Uncompress(messageBytes);


            messageBytes = AESCrypto.Decrypt(messageBytes, SecretKey, SecretVector);

            return LoginRequest.Parser.ParseFrom(messageBytes);
        }

    }
}