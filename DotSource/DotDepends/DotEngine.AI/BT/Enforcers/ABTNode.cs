using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public abstract class ABTNode
    {
        internal BTController Controller { get; set; }
        protected BTContext Context => Controller?.Context;

        protected BTNodeData NodeData { get; private set; }
        protected T GetNodeData<T>() where T : BTNodeData
        {
            return (T)NodeData;
        }

        public virtual void DoInitilize(BTNodeData nodeData)
        {
            NodeData = nodeData;
        }

        public virtual void DoDestroy()
        {
            NodeData = null;
        }
    }
}
