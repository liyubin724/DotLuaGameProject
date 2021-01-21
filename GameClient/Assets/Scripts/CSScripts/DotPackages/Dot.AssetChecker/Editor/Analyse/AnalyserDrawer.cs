﻿using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using System;
using System.Linq;
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
                    ShowMenuToCreateAnalyseRule((rule) =>
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

        void ShowMenuToCreateAnalyseRule(Action<IAnalyseRule> callback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && type.IsClass && typeof(IAnalyseRule).IsAssignableFrom(type)
                            select type).ToArray();
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(OperatationRuleAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    var attr = attrs[0] as OperatationRuleAttribute;
                    menu.AddItem(new GUIContent(attr.MenuItemName), false, () =>
                    {
                        if (Activator.CreateInstance(type) is IAnalyseRule rule)
                        {
                            callback?.Invoke(rule);
                        }
                    });
                }
            }
            menu.ShowAsContext();
        }
    }
}
