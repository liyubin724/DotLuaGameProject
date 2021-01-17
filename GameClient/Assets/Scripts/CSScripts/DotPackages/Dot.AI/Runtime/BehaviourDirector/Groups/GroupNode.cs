using System.Collections.Generic;

namespace DotEngine.BehaviourDirector.Nodes
{
    public class GroupNode : Node
    {
        public string Name;
        public float Duration = 0.0f;
        public List<TrackNode> Tracks = new List<TrackNode>();
    }
}
