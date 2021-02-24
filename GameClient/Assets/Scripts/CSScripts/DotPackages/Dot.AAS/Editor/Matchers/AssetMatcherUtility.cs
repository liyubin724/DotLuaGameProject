using DotEngine.Utilities;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS.Matchers
{
    public static class AssetMatcherUtility
    {
        private static Type[] matcherTypes = null;

        public static Type[] GetMatcherTypes()
        {
            if(matcherTypes == null)
            {
                matcherTypes = AssemblyUtility.GetDerivedTypes(typeof(IAssetMatcher));
            }
            return matcherTypes;
        }

        public static void ShowMenu(Action<IAssetMatcher> createCallback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = GetMatcherTypes();
            foreach(var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(CustomMatcherMenuAttribute), false);
                if(attrs!=null && attrs.Length>0)
                {
                    CustomMatcherMenuAttribute attr = attrs[0] as CustomMatcherMenuAttribute;
                    menu.AddItem(new GUIContent(attr.MenuPath), false, () =>
                    {
                        IAssetMatcher matcher = Activator.CreateInstance(type) as IAssetMatcher;
                        createCallback?.Invoke(matcher);
                    });
                }
            }
            menu.ShowAsContext();
        }
    }
}
