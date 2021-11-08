using System;
using UnityEngine;

namespace DotEngine.Net
{
    public class MessageDecoder : IMessageDecoder
    {
        public IMessageDecryptor Decryptor { get; set; }
        public IMessageUncompressor Uncompressor { get; set; }

        public unsafe bool Decode(byte[] dataBytes, out int id, out byte[] body)
        {
            body = null;
            id = -1;

            int offset = 0;
            int len = dataBytes.Length;
            fixed(byte* b = &dataBytes[0])
            {
                id = *((int*)(b+offset));
                offset += sizeof(int);
                if(len > offset)
                {
                    bool needDecrypt = *(b + offset) > 0;
                    if(needDecrypt && Decryptor == null)
                    {
                        Debug.LogError("the data can't be decrypted ,because the decryptor is NULL");
                        return false;
                    }
                    offset += 1;
                    bool needUncompress = *(b + offset) > 0;
                    if(needUncompress && Uncompressor == null)
                    {
                        Debug.LogError("the data can't be uncompressed ,because the uncompressor is NULL");
                        return false;
                    }

                    body = new byte[len - offset];
                    Array.Copy(dataBytes, offset, body, 0, body.Length);

                    if(needUncompress)
                    {
                        body = Uncompressor.Uncompress(body);
                    }
                    if(needDecrypt)
                    {
                        body = Decryptor.Decrypt(body);
                    }
                }
            }

            return true;
        }
    }
}
