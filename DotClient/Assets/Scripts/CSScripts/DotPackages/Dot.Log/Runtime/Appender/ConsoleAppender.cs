using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : ALogAppender
    {
        public static readonly string NAME = "Console";

        public ConsoleAppender() : base(NAME)
        { }

        protected override void LogMessage(LogLevel level, string message)
        {
            Console.WriteLine(message);
        }
    }
}