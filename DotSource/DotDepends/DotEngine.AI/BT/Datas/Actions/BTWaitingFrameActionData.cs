using DotEngine.AI.BT.Attributes;

namespace DotEngine.AI.BT.Datas
{
    [BTNodeMenuItem("Actions","Waiting Frame Action")]
    [BTNodeIdentification(1001,"WaitingFrameAction",Tooltips ="")]
    public class BTWaitingFrameActionData : BTActionNodeData
    {
        public int FrameCount = 0;
    }
}
