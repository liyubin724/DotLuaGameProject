using DotEngine.Utilities;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    public static class AssetReprocessUtility
    {
        private static Type[] reprocessTypes = null;
        public static Type[] GetReprocessTypes()
        {
            if(reprocessTypes == null)
            {
                reprocessTypes = AssemblyUtility.GetDerivedTypes(typeof(IAssetReprocess));
            }
            return reprocessTypes;
        }

        public static void ShowMenu(Action<IAssetReprocess> createCallback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = GetReprocessTypes();
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(CustomReprocessMenuAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    CustomReprocessMenuAttribute attr = attrs[0] as CustomReprocessMenuAttribute;
                    menu.AddItem(new GUIContent(attr.MenuPath), false, () =>
                    {
                        IAssetReprocess reprocess = Activator.CreateInstance(type) as IAssetReprocess;
                        createCallback?.Invoke(reprocess);
                    });
                }
            }
            menu.ShowAsContext();
        }
    }
}
