using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.AI.BT
{
    public class BTGraphView : GraphView
    {
        public BTEditorWindow Window { get; private set; }
        public Blackboard Blackboard { get; private set; }
        public MiniMap MiniMap { get; private set; }

        private BTNodeSearchProvider searchProvider;

        public BTGraphView(BTEditorWindow win)
        {
            StyleSheet sheetAsset = Resources.Load<StyleSheet>("BehaviourTree/Styles/BTStyles");
            styleSheets.Add(sheetAsset);

            Window = win;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            searchProvider = ScriptableObject.CreateInstance<BTNodeSearchProvider>();
            searchProvider.DoInitilize(this);
            nodeCreationRequest = (context) =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchProvider);
            };
        }

        public BTNodeView CreateNode(Type dataType,Vector2 mousePosition)
        {
            BTNodeData nodeData = Activator.CreateInstance(dataType) as BTNodeData;
            BTNodeView nodeView = new BTNodeView(this, nodeData);

            AddElement(nodeView);

            return nodeView;
        }
    }
}
