using System.Collections.Generic;

namespace DotEngine.BehaviourDirector.Nodes
{
    public class TrackNode :Node
    {
        public string Name = string.Empty;
        public List<ActionNode> Actions = new List<ActionNode>();
    }
}
