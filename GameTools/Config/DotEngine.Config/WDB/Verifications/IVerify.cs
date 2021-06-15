using DotEngine.Context;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    public interface IVerify
    {
        bool Verify(ref List<string> errors);
    }
}
