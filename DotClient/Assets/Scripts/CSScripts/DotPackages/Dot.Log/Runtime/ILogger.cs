using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Log
{
    public interface ILogger
    {
        string Tag { get; }
        ILogHandler Handler { get; set; }

        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
