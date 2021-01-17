using System;

namespace DotEngine.BehaviourDirector
{
    public enum NodeMode
    {
        Debug = 0,
        Release,
        All,
    }

    public enum NodePlatform
    {
        Client = 0,
        Server,
        All,
    }

    public abstract class Node 
    {
        public bool IsEnable = true;
    }
}
