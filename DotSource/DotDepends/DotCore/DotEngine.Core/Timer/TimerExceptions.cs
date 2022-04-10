using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Timer
{
    public class TimerException : Exception
    {
        public TimerException(string message) : base(message)
        {

        }
    }
}
