using System;

namespace DotEngine.Utilities
{
    public static class GuidUtility
    {
        public static Guid Parse(string input,string format = "N")
        {
            if(Guid.TryParseExact(input,format,out var result))
            {
                return result;
            }else
            {
                return Guid.Empty;
            }
        }

        public static string CreateNew()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
