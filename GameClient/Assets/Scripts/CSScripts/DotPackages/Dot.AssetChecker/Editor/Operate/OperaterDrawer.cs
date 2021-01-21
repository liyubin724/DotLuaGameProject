using DotEditor.GUIExt;
using DotEditor.GUIExt.NativeDrawer;
using System;
using System.Linq;
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
                listDrawer.IsShowInherit = false;

                listDrawer.CreateNewItem = () =>
                {
                    ShowMenuToCreateOperationRule((rule) =>
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

        void ShowMenuToCreateOperationRule(Action<IOperationRule> callback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && type.IsClass && typeof(IOperationRule).IsAssignableFrom(type)
                            select type).ToArray();
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(OperatationRuleAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    var attr = attrs[0] as OperatationRuleAttribute;
                    menu.AddItem(new GUIContent(attr.MenuItemName), false, () =>
                    {
                        if (Activator.CreateInstance(type) is IOperationRule rule)
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
