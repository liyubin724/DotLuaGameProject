using DotEngine.Entity.Avatar;
using DotEngine.Entity.Node;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar
{
    public class AvatarPreviewer
    {
        private GameObject avatarInstance = null;
        private NodeBehaviour nodeBehaviour = null;

        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();

        public AvatarPreviewer()
        {

        }

        public bool HasSkeleton()
        {
            return avatarInstance != null && nodeBehaviour != null;
        }

        public void LoadSkeleton(string skeletonPrefabPath)
        {
            UnloadSkeleton();

            GameObject skeletonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPrefabPath);
            if (skeletonPrefab != null)
            {
                avatarInstance = GameObject.Instantiate<GameObject>(skeletonPrefab);
                nodeBehaviour = avatarInstance.GetComponent<NodeBehaviour>();

                SceneView.lastActiveSceneView.LookAt(avatarInstance.transform.position);

                Selection.activeGameObject = avatarInstance;
            }
        }

        private void UnloadSkeleton()
        {
            nodeBehaviour = null;
            partInstanceDic.Clear();
            if (avatarInstance != null)
            {
                GameObject.DestroyImmediate(avatarInstance);
                avatarInstance = null;
            }
        }

        public void AddPart(string partAssetPath)
        {
            if (avatarInstance == null)
            {
                return;
            }

            AvatarPartData partData = AssetDatabase.LoadAssetAtPath<AvatarPartData>(partAssetPath);
            if (partData != null)
            {
                UnloadPart(partData.partType);

                partInstanceDic.Add(partData.partType, AvatarUtil.AssembleAvatarPart(nodeBehaviour, partData));
                AvatarUtil.AssembleAvatarPart(nodeBehaviour, partData);
            }
        }

        public void UnloadPart(AvatarPartType partType)
        {
            if (avatarInstance != null && partInstanceDic.TryGetValue(partType, out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance, true);
                partInstanceDic.Remove(partType);
            }
        }

        public void Dispose()
        {
            UnloadSkeleton();
        }
    }
}
