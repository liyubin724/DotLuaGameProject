using DotEditor.Utilities;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
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

        public static bool ImportAssets(string[] assetPaths,out string[] unknownAssetPaths)
        {
            List<string> results = new List<string>();
            AssetReprocessor[] reprocessors = AssetDatabaseUtility.FindInstances<AssetReprocessor>();
            if (reprocessors != null && reprocessors.Length > 0)
            {
                foreach(var assetPath in assetPaths)
                {
                    bool isReprocess = false;
                    foreach (var reprocessor in reprocessors)
                    {
                        if (reprocessor.IsMatch(assetPath))
                        {
                            reprocessor.Execute(assetPath);
                            isReprocess = true;
                            break;
                        }
                    }
                    if(!isReprocess)
                    {
                        results.Add(assetPath);
                    }
                }
            }
            unknownAssetPaths = results.ToArray();
            return results.Count == 0;
        }

        public static bool ImportAsset(string assetPath)
        {
            AssetReprocessor[] reprocessors = AssetDatabaseUtility.FindInstances<AssetReprocessor>();
            if (reprocessors != null && reprocessors.Length>0)
            {
                foreach(var reprocessor in reprocessors)
                {
                    if(reprocessor.IsMatch(assetPath))
                    {
                        reprocessor.Execute(assetPath);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
