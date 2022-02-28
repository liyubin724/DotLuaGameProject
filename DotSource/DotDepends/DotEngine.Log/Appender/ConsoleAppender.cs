using System;

namespace DotEngine.Log
{
    public class ConsoleAppender : LogAppender
    {
        public const string NAME = "Console";

        public ConsoleAppender(string name = NAME) : base(name)
        { }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage, string stacktrace)
        {
            string message = formattedMessage;
            if(!string.IsNullOrEmpty(stacktrace))
            {
                message = $"{formattedMessage}{Environment.NewLine}{stacktrace}";
            }
            Console.WriteLine(message);
        }
    }
}