using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Ini
{
    internal interface IDeepCopy<T> where T : class
    {
        T DeepCopy();
    }
}
