using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT
{
    public abstract class ABTComposeNode : ABTExecutorNode
    {
        protected List<ABTExecutorNode> nodes = new List<ABTExecutorNode>();


    }
}
