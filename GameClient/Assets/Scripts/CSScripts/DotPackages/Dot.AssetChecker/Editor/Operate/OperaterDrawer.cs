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
        private NArrayDrawer ruleArrayDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if (operater == null)
            {
                operater = ItemDrawer.Value as Operater;
                ruleArrayDrawer = new NArrayDrawer(operater.rules);
                ruleArrayDrawer.Header = "Operation Rules";
                ruleArrayDrawer.IsShowBox = true;

                ruleArrayDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateOperationRule((rule) =>
                    {
                        operater.rules.Add(rule);
                        ruleArrayDrawer.Refresh();
                    });
                };
                ruleArrayDrawer.ClearAllItem = () =>
                {
                    operater.rules.Clear();
                    ruleArrayDrawer.Refresh();
                };
                ruleArrayDrawer.DeleteItemAt = (index) =>
                {
                    operater.rules.RemoveAt(index);
                    ruleArrayDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                operater.enable = EditorGUILayout.Toggle("enable", operater.enable);
                ruleArrayDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
