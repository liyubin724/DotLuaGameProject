using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    [CustomTypeDrawer(typeof(Operater))]
    public class OperaterDrawer : NTypeDrawer
    {
        private Operater operater = null;
        private NArrayDrawer listDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if (operater == null)
            {
                operater = ItemDrawer.Value as Operater;
                listDrawer = new NArrayDrawer(operater.rules);
                listDrawer.Header = "Operation Rules";
                listDrawer.IsShowBox = true;
                listDrawer.IsShowInherit = true;

                listDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateOperationRule((rule) =>
                    {
                        operater.rules.Add(rule);
                        listDrawer.Refresh();
                    });
                };
                listDrawer.ClearAllItem = () =>
                {
                    operater.rules.Clear();
                    listDrawer.Refresh();
                };
                listDrawer.DeleteItemAt = (index) =>
                {
                    operater.rules.RemoveAt(index);
                    listDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                operater.enable = EditorGUILayout.Toggle("enable", operater.enable);
                listDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
