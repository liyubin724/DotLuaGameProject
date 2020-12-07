using DotEngine.AI.BD.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BD.Tracks
{
    public enum NodeTrackCategory
    {
        None = 0,

        Actor,

        Max,
    }

    public class Track
    {
        public string Name = string.Empty;
        public NodeTrackCategory Category = NodeTrackCategory.None;
        public List<ActionNode> Nodes = new List<ActionNode>();

        public void DoUpdate(float deltaTime)
        {

        }
    }
}
