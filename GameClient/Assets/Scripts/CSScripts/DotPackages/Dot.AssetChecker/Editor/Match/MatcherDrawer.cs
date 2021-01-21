using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using System;
using System.Linq;
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
                    ShowMenuToCreateMatchFilter((filter) =>
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

        void ShowMenuToCreateMatchFilter(Action<IMatchFilter> callback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && type.IsClass && typeof(IMatchFilter).IsAssignableFrom(type)
                            select type).ToArray();
            foreach (var type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MatchFilterAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    MatchFilterAttribute attr = attrs[0] as MatchFilterAttribute;
                    menu.AddItem(new GUIContent(attr.MenuItemName), false, () =>
                    {
                        if (Activator.CreateInstance(type) is IMatchFilter filter)
                        {
                            callback?.Invoke(filter);
                        }
                    });
                }
            }
            menu.ShowAsContext();
        }
    }
}
