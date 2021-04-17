using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : ALogAppender
    {
        public static readonly string NAME = "Console";

        public ConsoleAppender() : this(LogLevelConst.All)
        {
        }

        public ConsoleAppender(LogLevel validLevel) : base(NAME, validLevel)
        { }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            Console.WriteLine(message);
        }
    }
}