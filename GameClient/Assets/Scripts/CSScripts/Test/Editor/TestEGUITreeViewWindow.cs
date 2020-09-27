using DotEditor.GUIExtension.TreeGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace ETest
{
    public class TestTreeViewData : TreeViewData
    {
        public string name;
        public List<string> childNames = new List<string>();

        public override string GetDisplayName()
        {
            return name;
        }

        public static TestTreeViewData Root
        {
            get
            {
                return new TestTreeViewData()
                {
                    name = string.Empty,
                };
            }
        }
    }

    public class TestTreeViewModel : TreeViewModel
    {
        public TestTreeViewModel(TestTreeViewData root) : base(root)
        {
        }

        public override bool HasChild(TreeViewData data)
        {
            return ((TestTreeViewData)data).childNames.Count > 0;
        }

        protected override void OnExpandData(TreeViewData data)
        {
            var d = (TestTreeViewData)data;
            foreach(var n in d.childNames)
            {
                AddChild(data, new TestTreeViewData()
                {
                    name = n,
                });
            }
        }
    }

    public class TestEGUITreeViewWindow : EditorWindow
    {
        [MenuItem("Test/TreeView")]
        public static void ShowWin()
        {
            var win = GetWindow<TestEGUITreeViewWindow>();
            win.Show();
        }

        EGUITreeView treeView = null;
        TestTreeViewModel model = null;
        private void Awake()
        {
            model = new TestTreeViewModel(TestTreeViewData.Root);

            model.AddChild(model.RootData, new TestTreeViewData()
            {
                name = "test",
                childNames = new List<string>()
                {
                    "A","B","C","D"
                },
            });

            model.AddChild(model.RootData, new TestTreeViewData()
            {
                name = "test1",
                childNames = new List<string>()
                {
                    "A1","B1","C1","D1"
                },
            });

        }

        private void OnGUI()
        {
            if(treeView == null)
            {
                treeView = new EGUITreeView(new TreeViewState(), model);
                treeView.Reload();
            }

            treeView.OnGUI(new UnityEngine.Rect(0,0,position.width,position.height));
        }


    }
}
