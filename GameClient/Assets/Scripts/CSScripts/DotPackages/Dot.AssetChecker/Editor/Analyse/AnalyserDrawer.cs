using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    [CustomTypeDrawer(typeof(Analyser))]
    public class AnalyserDrawer : NTypeDrawer
    {
        private Analyser analyser = null;
        private NArrayDrawer ruleArrayDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if (analyser == null)
            {
                analyser = ItemDrawer.Value as Analyser;
                ruleArrayDrawer = new NArrayDrawer(analyser.rulers);
                ruleArrayDrawer.Header = "Filters";
                ruleArrayDrawer.IsShowBox = true;

                ruleArrayDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateAnalyseRule((rule) =>
                    {
                        analyser.rulers.Add(rule);
                        ruleArrayDrawer.Refresh();
                    });
                };
                ruleArrayDrawer.ClearAllItem = () =>
                {
                    analyser.rulers.Clear();
                    ruleArrayDrawer.Refresh();
                };
                ruleArrayDrawer.DeleteItemAt = (index) =>
                {
                    analyser.rulers.RemoveAt(index);
                    ruleArrayDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                analyser.enable = EditorGUILayout.Toggle("enable", analyser.enable);
                ruleArrayDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
