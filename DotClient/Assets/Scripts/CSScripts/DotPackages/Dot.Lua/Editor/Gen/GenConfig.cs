using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenConfig : ScriptableObject
    {
        public List<string> AssemblyNames = new List<string>();

        public List<string> callCSharpTypeNames = new List<string>();
        public List<string> callLuaTypeNames = new List<string>();
        public List<string> optimizeTypeNames = new List<string>();

        public List<string> callCSharpGenericTypeNames = new List<string>();
        public List<string> callLuaGenericTypeNames = new List<string>();

        public List<string> blackDatas = new List<string>();

        private static string GEN_CONFIG_ASSET_PATH = "Assets/Settings/Lua/gen_config.asset";
        public static GenConfig GetConfig(bool createIfNotExist = true)
        {
            GenConfig genConfig = AssetDatabase.LoadAssetAtPath<GenConfig>(GEN_CONFIG_ASSET_PATH);
            if (genConfig == null && createIfNotExist)
            {
                genConfig = ScriptableObject.CreateInstance<GenConfig>();
                AssetDatabase.CreateAsset(genConfig, GEN_CONFIG_ASSET_PATH);
                AssetDatabase.ImportAsset(GEN_CONFIG_ASSET_PATH);
            }
            return genConfig;
        }
    }
}
