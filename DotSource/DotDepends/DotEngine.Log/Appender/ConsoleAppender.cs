using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : AFormatLogAppender
    {
        public const string NAME = "Console";

        public ConsoleAppender(string name = NAME, ILogFormatter formatter = null) : base(name, formatter)
        { }

        protected override void OutputFormattedMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}