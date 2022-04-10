namespace DotEngine.Config.Ini
{
    public class IniReaderStyle
    {
        /// <summary>
        /// 读取时是否读取注释信息
        /// </summary>
        public bool IsParseComments { get; set; } = true;
        /// <summary>
        /// 是否去除读取到的注释信息开始与结尾处的空白字符
        /// </summary>
        public bool IsTrimComments { get; set; } = true;
        /// <summary>
        /// 是否读取属性的可选值
        /// </summary>
        public bool IsParseOptionalValues { get; set; } = true;
        /// <summary>
        /// 是否去除可选值开始与结尾处的空白字符
        /// </summary>
        public bool IsTrimOptionalValues { get; set; } = true;
        
        public bool IsTrimPropertyKey { get; set; } = true;
        public bool IsTrimPropertyValue { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public bool IsTrimSections { get; set; } = true;
    }
}
