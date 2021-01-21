using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    [CustomTypeDrawer(typeof(Matcher))]
    public class MatcherDrawer : NTypeDrawer
    {
        private Matcher matcher = null;
        private NArrayDrawer listDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if(matcher == null)
            {
                matcher = ItemDrawer.Value as Matcher;
                listDrawer = new NArrayDrawer(matcher.filters);
                listDrawer.Header = "Filters";
                listDrawer.IsShowBox = true;
                listDrawer.IsShowInherit = true;
                
                listDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateMatchFilter((filter) =>
                    {
                        matcher.filters.Add(filter);
                        listDrawer.Refresh();
                    });
                };
                listDrawer.ClearAllItem = () =>
                {
                    matcher.filters.Clear();
                    listDrawer.Refresh();
                };
                listDrawer.DeleteItemAt = (index) =>
                {
                    matcher.filters.RemoveAt(index);
                    listDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                matcher.enable = EditorGUILayout.Toggle("enable", matcher.enable);
                listDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
