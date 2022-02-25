using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class ABTComposeNode : BTAExecutorNode
    {
        protected List<BTAExecutorNode> nodes = new List<BTAExecutorNode>();


    }
}
