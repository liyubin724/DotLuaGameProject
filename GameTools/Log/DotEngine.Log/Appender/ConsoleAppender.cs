using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : ALogAppender
    {
        public ConsoleAppender() : base(typeof(ConsoleAppender).Name)
        {
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            Console.WriteLine(message);
        }
    }
}
