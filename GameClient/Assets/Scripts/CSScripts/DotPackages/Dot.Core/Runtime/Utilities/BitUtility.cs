namespace DotEngine.Utilities
{
    public static class BitUtility
    {
        /// <summary>
        /// 对Byte的数据根据指定的字节进行操作
        /// </summary>
        /// <param name="input">需要设置的数据</param>
        /// <param name="index">需要操作的字节位置</param>
        /// <param name="enable">enable为true时，指定位置的字节设置为1，否则为0</param>
        /// <returns></returns>
        public static byte SetBit(byte input, int index, bool enable)
        {
            if (enable)
            {
                return (byte)(input | (1 << index));
            }
            else
            {
                return (byte)(input & (~(1 << index)));
            }
        }

        public static byte GetBit(byte input,int index)
        {
            return (byte)(input & (1 << index));
        }

        public static bool IsEnable(byte input ,int index)
        {
            return GetBit(input, index) > 0;
        }

        public static int SetBit(int input,int index,bool enable)
        {
            if(enable)
            {
                return input | (1 << index);
            }else
            {
                return input & (~(1 << index));
            }
        }

        public static int GetBit(int input,int index)
        {
            return input & (1 << index);
        }

        public static bool IsEnable(int input,int index)
        {
            return GetBit(input, index) > 0;
        }
    }
}
