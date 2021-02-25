using DotEditor.AAS.Matchers;
using Newtonsoft.Json;
using UnityEngine;
using System;
using DotEditor.GUIExt.NativeDrawer;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.AAS.Reprocessor
{
    [CreateAssetMenu(fileName ="asset_reprocess",menuName = "AAS/Reprocessor")]
    [CustomDrawerEditor(IsShowBox =true,IsShowInherit =false,IsShowTargetType =true)]
    public class AssetReprocessor : ScriptableObject, IAssetMatcher, IAssetReprocess,ISerializationCallbackReceiver
    {
        public bool enable = true;
        [Hide]
        public string matcherJson = string.Empty;
        [Hide]
        public string reprocessJson = string.Empty;
         
        [NonSerialized]
        public ComposedMatcher matcher = new ComposedMatcher();
        [NonSerialized]
        public ComposedReprocess reprocess = new ComposedReprocess();
         
        public void Execute(string assetPath)
        {
            reprocess.Execute(assetPath);
        }

        public bool IsMatch(string assetPath)
        {
            return matcher.IsMatch(assetPath);
        }

        public void OnAfterDeserialize()
        {
            if(!string.IsNullOrEmpty(matcherJson))
            {
                matcher = JsonConvert.DeserializeObject<ComposedMatcher>(matcherJson, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            if(!string.IsNullOrEmpty(reprocessJson))
            {
                reprocess = JsonConvert.DeserializeObject<ComposedReprocess>(reprocessJson, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
        }
        
        public void OnBeforeSerialize()
        {
            matcherJson = JsonConvert.SerializeObject(matcher, Formatting.None, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            reprocessJson = JsonConvert.SerializeObject(reprocess,Formatting.None, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
