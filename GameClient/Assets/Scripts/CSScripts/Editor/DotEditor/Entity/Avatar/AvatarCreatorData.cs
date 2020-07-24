using DotEngine.Entity.Avatar;
using DotEngine.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorData : ScriptableObject
    {
        public AvatarSkeletonCreatorData skeletonData = new AvatarSkeletonCreatorData();
        public AvatarSkeletonPartCreatorData skeletonPartData = new AvatarSkeletonPartCreatorData();

        public override string ToString()
        {
            return name;
        }

        [Serializable]
        public class AvatarSkeletonCreatorData
        {
            [OpenFolderPath]
            public string outputFolder = string.Empty;
            public GameObject fbx;

            public string GetSkeletonPrefabPath()
            {
                if (string.IsNullOrEmpty(outputFolder) || fbx == null)
                {
                    return null;
                }
                return $"{outputFolder}/{fbx.name}_skeleton.prefab";
            }
        }

        [Serializable]
        public class AvatarSkeletonPartCreatorData
        {
            [OpenFolderPath]
            public string outputFolder = string.Empty;
            public List<AvatarPartCreatorData> partDatas = new List<AvatarPartCreatorData>();
        }

        [Serializable]
        public class AvatarPartCreatorData
        {
            public string name = "";
            [EnumButton]
            public AvatarPartType partType = AvatarPartType.Body;

            public List<AvatarPartSMRendererCreatorData> rendererDatas = new List<AvatarPartSMRendererCreatorData>();
            public List<AvatarPartPrefabCreatorData> prefabDatas = new List<AvatarPartPrefabCreatorData>();

            public string GetPartAssetPath(string outputDir)
            {
                return $"{outputDir}/{name}_part.asset";
            }
        }

        [Serializable]
        public class AvatarPartPrefabCreatorData
        {
            public string bindNodeName = string.Empty;
            public GameObject bindPrefab = null;
        }

        [Serializable]
        public class AvatarPartSMRendererCreatorData
        {
            public bool IsCopyMesh = true;
            public GameObject fbx;
        }
    }
}
