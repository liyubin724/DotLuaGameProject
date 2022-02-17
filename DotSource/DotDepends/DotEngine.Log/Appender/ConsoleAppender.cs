using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : LogAppender
    {
        public static readonly string NAME = "Console";

        public ConsoleAppender() : base(NAME)
        { }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage)
        {
            Console.WriteLine(formattedMessage);
        }
    }
}