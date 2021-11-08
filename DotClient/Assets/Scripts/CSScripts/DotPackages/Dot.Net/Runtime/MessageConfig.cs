using DotEngine.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Net
{
    public class MessageConfig : ISerialization
    {
        public List<MessageData> Datas = new List<MessageData>();

        private Dictionary<int, MessageData> dataDic = new Dictionary<int, MessageData>();

        public MessageData GetData(int messageId)
        {
            if (dataDic.TryGetValue(messageId, out var data))
            {
                return data;
            }
            return null;
        }

        public void DoDeserialize()
        {
            foreach (var data in Datas)
            {
                dataDic.Add(data.Id, data);
            }
        }

        public void DoSerialize()
        {
            Datas = dataDic.Values.ToList();
        }
    }

    public class MessageData
    {
        public int Id;
        public string Name;
        public string Desc;
        public int CompressType;
        public int UncompressType;
        public int EncryptorType;
        public int DecryptorType;
    }
}
