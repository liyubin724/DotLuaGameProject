using DotEngine.Core.Utilities;
using ReflectionMagic;
using System;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace DotEditor.Utilities
{
    public static class PrefabUtility
    {
        public static bool IsPrefab(string assetPath)
        {
            if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public static bool IsMissingNestPrefab(string assetPath)
        {
            if(!IsPrefab(assetPath))
            {
                return false;
            }

            OpenPrefabStage(assetPath);

            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            GameObject rootGO = stage.prefabContentsRoot;
            Transform[] transforms = rootGO.GetComponentsInChildren<Transform>();
            foreach (var t in transforms)
            {
                if (t.name.IndexOf("Missing Prefab") >= 0)
                {
                    return true;
                }
            }

            ClosePrefabStage();

            return false;
        }

        public static bool IsInPrefabStage()
        {
            var stage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null;
        }

        public static PrefabStage OpenPrefabStage(string prefabPath)
        {
            if(IsPrefab(prefabPath))
            {
                GameObject prefabGO = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if(prefabGO!=null)
                {
                    dynamic utilityDynamic = typeof(PrefabStageUtility).AsDynamicType();
                    utilityDynamic.OpenPrefab(prefabPath);

                    return PrefabStageUtility.GetCurrentPrefabStage();
                }
            }

            return null;
        }

        public static void ClosePrefabStage()
        {
            if(IsInPrefabStage())
            {
                Type changeType = AssemblyUtility.GetType("UnityEditor.SceneManagement.StageNavigationManager+Analytics+ChangeType");
                var changeTypeValue = Enum.Parse(changeType, "NavigateBackViaHierarchyHeaderLeftArrow");

                Type type = AssemblyUtility.GetType("UnityEditor.SceneManagement.StageNavigationManager");
                dynamic typeDynamic = type.AsDynamicType();
                var snMgr = typeDynamic.instance;
                snMgr.NavigateBack(changeTypeValue);
            }
        }
    }
}
