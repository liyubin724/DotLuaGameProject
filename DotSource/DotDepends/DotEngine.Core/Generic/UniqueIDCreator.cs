using System;
using System.Collections.Generic;

namespace DotEngine.Generic
{
    public interface IUniqueIDCreator<T>
    {
        T GetNextID();
    }

    public class GUIDCreator : IUniqueIDCreator<string>
    {
        public static GUIDCreator Default = new GUIDCreator();

        private Dictionary<string, bool> guidDic = new Dictionary<string, bool>();
        public GUIDCreator()
        {
        }

        public string GetNextID()
        {
            Guid guid = Guid.NewGuid();
            string strGuid = guid.ToString("N");
            if (guidDic.ContainsKey(strGuid))
            {
                strGuid = GetNextID();
            }

            guidDic.Add(strGuid, true);

            return strGuid;
        }
    }

    public class IntIDCreator : IUniqueIDCreator<int>
    {
        public static IntIDCreator Default = new IntIDCreator(0);

        private int currentId = 0;
        public IntIDCreator(int start = 0)
        {
            currentId = start;
        }

        public int GetNextID()
        {
            return ++currentId;
        }
    }

    public class LongIDCreator : IUniqueIDCreator<long>
    {
        public static LongIDCreator Default = new LongIDCreator(0);

        private long currentId = 0;
        public LongIDCreator(long start = 0)
        {
            currentId = start;
        }

        public long GetNextID()
        {
            return ++currentId;
        }
    }

    public class SnowflakeIDCreator : IUniqueIDCreator<long>
    {
        public long GetNextID()
        {
            throw new NotImplementedException();
        }
    }
}
