using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    internal static class CheckerUtility
    {
        internal static void ShowMenuToCreateMatchFilter(Action<IMatchFilter> callback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && type.IsClass && typeof(IMatchFilter).IsAssignableFrom(type)
                            select type).ToArray();
            foreach(var type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MatchFilterAttribute), false);
                if(attrs!=null && attrs.Length>0)
                {
                    MatchFilterAttribute attr = attrs[0] as MatchFilterAttribute;
                    menu.AddItem(new GUIContent(attr.MenuItemName), false,() =>
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

        internal static void ShowMenuToCreateOperationRule(Action<IOperationRule> callback)
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

        internal static void ShowMenuToCreateAnalyseRule(Action<IAnalyseRule> callback)
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
