using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace DotEditor.AI.BT
{
    public enum BTEPortLinkDirection
    {
        Parent = 0,
        Child,
    }

    public class BTNodeView : Node
    {
        public BTGraphView GraphView { get; private set; }
        public BTNodeData NodeData { get; private set; }
        public BTNodeView(BTGraphView graphView,BTNodeData nodeData):base()
        {
            GraphView = graphView;
            NodeData = nodeData;

            Type dataType = nodeData.GetType();
            title = dataType.Name;


        }


        public void CreatePort(BTEPortLinkDirection linkDirection)
        {
            var direction = Direction.Input;
            var container = inputContainer;
            if(linkDirection == BTEPortLinkDirection.Child)
            {
                direction = Direction.Output;
                container = outputContainer;
            }

        }
    }
}
