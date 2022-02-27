using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace DotEditor.AI.BT
{
    public class BTNodeView : Node
    {
        public BTGraphView GraphView { get; private set; }
        public BTNodeData NodeData { get; private set; }
        public BTNodeView(BTGraphView graphView,BTNodeData nodeData):base()
        {
            GraphView = graphView;
            NodeData = nodeData;
        }

    }
}
