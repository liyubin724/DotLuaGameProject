﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public interface IMessageDeserializer
    {
        void OnDataReceived(byte[] bytes, int size);
    }
}
