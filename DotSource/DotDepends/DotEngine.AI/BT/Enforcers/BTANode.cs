using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTANode
    {
        public BTController Controller { get; private set; }
        public BTContext Context => Controller?.Context;

        protected BTNodeData NodeData { get; private set; }
        protected T GetNodeData<T>() where T : BTNodeData
        {
            return (T)NodeData;
        }

        public virtual void DoInitilize(BTController controller, BTNodeData data)
        {
            Controller = controller;
            NodeData = data;
        }

        public virtual void DoDestroy()
        {
            Controller = null;

            NodeData = null;
        }
    }
}
