using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : AFormatLogAppender
    {
        public ConsoleAppender(string name, ILogFormatter formatter = null) : base(name, formatter)
        { }

        protected override void OutputFormattedMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}