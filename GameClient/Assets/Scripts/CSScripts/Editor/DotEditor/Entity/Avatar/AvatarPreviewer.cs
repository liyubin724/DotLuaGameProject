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

        private Dictionary<string, AvatarPartInstance> partInstanceDic = new Dictionary<string, AvatarPartInstance>();

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
                UnloadPart(partData.partName);

                partInstanceDic.Add(partData.partName, AvatarUtil.AssemblePart(nodeBehaviour, partData));
                AvatarUtil.AssemblePart(nodeBehaviour, partData);
            }
        }

        public void UnloadPart(string partName)
        {
            if (avatarInstance != null && partInstanceDic.TryGetValue(partName, out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassemblePart(partInstance);
                partInstanceDic.Remove(partName);
            }
        }

        public void Dispose()
        {
            UnloadSkeleton();
        }
    }
}
