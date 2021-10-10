using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Log
{
    public interface ILogHandler
    {
        void OnLogReceived(string tag, LogLevel logLevel, string message);
    }
}
