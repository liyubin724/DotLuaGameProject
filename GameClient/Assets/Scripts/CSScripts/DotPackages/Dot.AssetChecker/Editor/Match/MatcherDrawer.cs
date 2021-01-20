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
        private NArrayDrawer filterArrayDrawer = null;
        public override void OnGUILayout()
        {
            EGUILayout.DrawBoxHeader(Label, GUILayout.ExpandWidth(true));
            if(matcher == null)
            {
                matcher = ItemDrawer.Value as Matcher;
                filterArrayDrawer = new NArrayDrawer(matcher.filters);
                filterArrayDrawer.Header = "Filters";
                filterArrayDrawer.IsShowBox = true;
                
                filterArrayDrawer.CreateNewItem = () =>
                {
                    CheckerUtility.ShowMenuToCreateMatchFilter((filter) =>
                    {
                        matcher.filters.Add(filter);
                        filterArrayDrawer.Refresh();
                    });
                };
                filterArrayDrawer.ClearAllItem = () =>
                {
                    matcher.filters.Clear();
                    filterArrayDrawer.Refresh();
                };
                filterArrayDrawer.DeleteItemAt = (index) =>
                {
                    matcher.filters.RemoveAt(index);
                    filterArrayDrawer.Refresh();
                };
            }
            EGUI.BeginIndent();
            {
                matcher.enable = EditorGUILayout.Toggle("enable", matcher.enable);
                filterArrayDrawer.OnGUILayout();
            }
            EGUI.EndIndent();

        }
    }
}
