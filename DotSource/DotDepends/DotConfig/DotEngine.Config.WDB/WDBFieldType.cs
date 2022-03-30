using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    public enum WDBFieldType
    {
        None = 0,

        Int = 'i',
        Ref = 'r',
        Long = 'l',
        Bool = 'b',
        Float = 'f',
        String = 's',
        UAsset = 'u',
    }
}
