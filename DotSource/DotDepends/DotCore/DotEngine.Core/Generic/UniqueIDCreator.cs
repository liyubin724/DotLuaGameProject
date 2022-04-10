using DotEngine.Utilities;
using System;

namespace DotEngine.Generic
{
    public interface IUniqueIDCreator<T>
    {
        T GetNextID();
    }

    public class GuidCreator : IUniqueIDCreator<string>
    {
        public static GuidCreator Default = new GuidCreator();
        public GuidCreator()
        {
        }

        public string GetNextID()
        {
            return GuidUtility.CreateNew();
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
