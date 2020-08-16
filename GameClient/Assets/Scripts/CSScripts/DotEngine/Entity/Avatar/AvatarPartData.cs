using DotEngine.NativeDrawer.Property;
using System;
using UnityEngine;

namespace DotEngine.Entity.Avatar
{
    public class AvatarPartData : ScriptableObject
    {
        [Readonly]
        public string partName = string.Empty;
        [Readonly]
        public RendererPartData[] rendererParts = new RendererPartData[0];
        [Readonly]
        public PrefabPartData[] prefabParts = new PrefabPartData[0];

        [Serializable]
        public class RendererPartData
        {
            public string rendererName = "";

            public string rootBoneName = "";
            public string[] boneNames = new string[0];

            public Mesh mesh = null;
            public Material[] materials = new Material[0];
        }

        [Serializable]
        public class PrefabPartData
        {
            public string bindName = "";
            public GameObject prefabGO = null;
        }
    }
}
