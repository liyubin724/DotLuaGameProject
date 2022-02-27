using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DotEditor.AI.BT
{
    public class BTNodeSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private BTGraphView graphView;
        public void DoInitilize(BTGraphView graphView)
        {
            this.graphView = graphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            SearchTreeGroupEntry rootGroupEntry = new SearchTreeGroupEntry(new GUIContent("Behaviour Tree Nodes"))
            {
                level = 0
            };
            SearchTreeGroupEntry gEntry = new SearchTreeGroupEntry(new GUIContent("Actions")) { level = 1 };
            SearchTreeEntry entry = new SearchTreeEntry(new GUIContent("Waiting Frame Action")) { level = 2 };
            return new List<SearchTreeEntry>() { rootGroupEntry,gEntry, entry };
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            graphView.CreateNode(typeof(BTWaitingFrameActionData), context.screenMousePosition);
            return true;
        }
    }
}
