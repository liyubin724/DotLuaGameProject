using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Utilities
{
    public static class GraphicsSettingsUtility
    {
        public static GraphicsSettings Setting
        {
            get
            {
                return AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0] as GraphicsSettings;
            }
        }

        public static SerializedProperty GetAlawysIncludeShadersProperty()
        {
            SerializedObject graphicsSettings = new SerializedObject(Setting);
            SerializedProperty it = graphicsSettings.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "m_AlwaysIncludedShaders")
                {
                    return it;
                }
            }
            return null;
        }

        public static string[] GetAlawysIncludeShaders()
        {
            SerializedProperty includeShadersProperty = GetAlawysIncludeShadersProperty();
            if (includeShadersProperty == null) return null;
            List<string> shaderList = new List<string>();
            for(int i =0;i<includeShadersProperty.arraySize;++i)
            {
                UnityObject shaderObj = includeShadersProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                if(shaderObj!=null)
                {
                    shaderList.Add(AssetDatabase.GetAssetPath(shaderObj));
                }
            }
            return shaderList.ToArray();
        }

        public static void SetAlawysIncludeShaders(string[] shaders)
        {
            SerializedProperty includeShadersProperty = GetAlawysIncludeShadersProperty();
            if (includeShadersProperty == null) return;

            includeShadersProperty.ClearArray();
            SerializedProperty dataProperty = null;
            if(shaders!=null && shaders.Length>0)
            {
                for (int i = 0; i < shaders.Length; ++i)
                {
                    includeShadersProperty.InsertArrayElementAtIndex(i);
                    dataProperty = includeShadersProperty.GetArrayElementAtIndex(i);
                    dataProperty.objectReferenceValue = AssetDatabase.LoadAssetAtPath<Shader>(shaders[i]);
                }

                includeShadersProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
