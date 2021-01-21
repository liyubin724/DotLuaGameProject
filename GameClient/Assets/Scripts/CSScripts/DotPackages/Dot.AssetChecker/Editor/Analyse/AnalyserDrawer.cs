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
        private NArrayDrawer listDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if (analyser == null)
            {
                analyser = ItemDrawer.Value as Analyser;
                listDrawer = new NArrayDrawer(analyser.rulers);
                listDrawer.Header = "Filters";
                listDrawer.IsShowBox = true;
                listDrawer.IsShowInherit = true;

                listDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateAnalyseRule((rule) =>
                    {
                        analyser.rulers.Add(rule);
                        listDrawer.Refresh();
                    });
                };
                listDrawer.ClearAllItem = () =>
                {
                    analyser.rulers.Clear();
                    listDrawer.Refresh();
                };
                listDrawer.DeleteItemAt = (index) =>
                {
                    analyser.rulers.RemoveAt(index);
                    listDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                analyser.enable = EditorGUILayout.Toggle("enable", analyser.enable);
                listDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
