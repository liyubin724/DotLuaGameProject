using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public interface IMessageCryptor
    {
        byte[] Encode(byte[] datas);
        byte[] Decode(byte[] datas);
    }
}
