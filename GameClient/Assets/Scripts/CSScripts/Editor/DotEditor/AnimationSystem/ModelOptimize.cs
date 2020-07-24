using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AnimationSystem
{
    public static class ModelOptimize
    {
        //[MenuItem("Test/Model Undo Optimize")]
        //public static void Undooptimize()
        //{
        //    UnityObject uObj = Selection.activeObject;
        //    string assetPath = AssetDatabase.GetAssetPath(uObj);

        //    UnoptimizeGameObjects(assetPath);
        //}

        //[MenuItem("Test/Model Optimize")]
        //public static void Optimize()
        //{
        //    UnityObject uObj = Selection.activeObject;
        //    string assetPath = AssetDatabase.GetAssetPath(uObj);

        //    OptimizeGameObjects(assetPath, new string[] { "Bip001 R Toe0" });
        //}

        public static void UnoptimizeGameObjects(string assetPath)
        {
            ModelImporter modelImporter = (ModelImporter)ModelImporter.GetAtPath(assetPath);
            if (modelImporter == null)
            {
                return;
            }
            modelImporter.optimizeGameObjects = false;
            modelImporter.extraExposedTransformPaths = new string[0];
            modelImporter.SaveAndReimport();
        }

        public static void OptimizeGameObjects(string assetPath,string[] extraExposedTransformNames)
        {
            ModelImporter modelImporter = (ModelImporter)ModelImporter.GetAtPath(assetPath);
            if(modelImporter == null)
            {
                Debug.LogError("ModelOptimize::OptimizeGameObjects->modelImporter is null.assetPath = " + assetPath);
                return;
            }

            if(modelImporter.animationType != ModelImporterAnimationType.Human && modelImporter.animationType != ModelImporterAnimationType.Generic)
            {
                Debug.LogError("ModelOptimize::OptimizeGameObjects->animationType is not Human or Generic. animtionType = " + modelImporter.animationType);
                return;
            }

            SerializedObject serializedObject = new SerializedObject(modelImporter);
            SerializedProperty copyAvatar = serializedObject.FindProperty("m_CopyAvatar");
            if(copyAvatar == null || copyAvatar.boolValue)
            {
                Debug.LogError("ModelOptimize::OptimizeGameObjects-> Avatar Definition should be set as \"Create From This Model\"");
                return;
            }

            modelImporter.optimizeGameObjects = true;
            if(extraExposedTransformNames == null || extraExposedTransformNames.Length == 0)
            {
                modelImporter.extraExposedTransformPaths = new string[0];
                modelImporter.SaveAndReimport();
                return;
            }

            List<string> paths = new List<string>();
            string[] transformPaths = modelImporter.transformPaths;
            foreach(var path in transformPaths)
            {
                foreach(var name in extraExposedTransformNames)
                {
                    if(path.ToLower().Contains(name.ToLower()))
                    {
                        paths.Add(path);
                    }
                }
            }

            modelImporter.extraExposedTransformPaths = paths.ToArray();
            modelImporter.SaveAndReimport();

            AssetDatabase.ImportAsset(assetPath);
        }
    }
}
