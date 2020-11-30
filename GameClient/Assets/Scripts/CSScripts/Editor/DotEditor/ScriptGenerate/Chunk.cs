namespace DotTool.ScriptGenerate
{
    public enum TokenType
    {
        None = 0,
        /// <summary>
        /// 在标记<%-XXXX%>中的内容将会被忽略
        /// </summary>
        Ignore,
        /// <summary>
        /// 标记在<%+using System;%>中的将会添加到Using中
        /// </summary>
        Using,
        /// <summary>
        /// 标记在<%XXXX%>中的将会做为代码运行
        /// </summary>
        Code,
        /// <summary>
        /// 标记在<%=XXXX%>中的将会被做为表达式处理
        /// </summary>
        Eval,
        /// <summary>
        /// 普通的文本内容
        /// </summary>
        Text,
    }

    public class Chunk
    {
        public TokenType Type { get; private set; }
        public string Text { get; private set; }

        public Chunk(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}
