using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;

namespace DotEngine.Assets
{
    public class AssetAsyncRequest
    {
        public string[] Addresses = new string[0];
        public string[] Paths = new string[0];
        public string Label;

        public bool IsInstance = false;

        public SystemObject Userdata = null;

    }
}
