using DotEngine.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Net
{
    public class ProtoMessageConfig : ISerialization
    {
        public List<ProtoMessageData> Datas = new List<ProtoMessageData>();

        private Dictionary<int, ProtoMessageData> dataDic = new Dictionary<int, ProtoMessageData>();

        public ProtoMessageData GetData(int messageId)
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

    public class ProtoMessageData
    {
        public int Id;
        public string Name;
        public string Desc;
        public bool IsCompress = false;
        public bool IsEncrypt = false;
    }
}
