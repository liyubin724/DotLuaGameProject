using DotEngine.Entity.Node;
using DotEngine.Log;
using UnityEngine;

namespace DotEngine.Entity.Avatar
{
    public static class AvatarUtil
    {
        public static AvatarPartInstance AssemblePart(NodeBehaviour nodeBehaviour, AvatarPartData partData)
        {
            AvatarPartInstance partInstance = new AvatarPartInstance();
            partInstance.partName = partData.partName;
            partInstance.gameObjects = new GameObject[partData.prefabParts.Length];
            for (int i = 0; i < partData.prefabParts.Length; ++i)
            {
                var prefabData = partData.prefabParts[i];

                GameObject bindGameObject = GameObject.Instantiate(prefabData.prefabGO);
                Transform bindNodeTran = nodeBehaviour.GetBindTransform(prefabData.bindName);
                bindGameObject.transform.SetParent(bindNodeTran, false);

                partInstance.gameObjects[i] = bindGameObject;
            }

            partInstance.renderers = new SkinnedMeshRenderer[partData.rendererParts.Length];
            for (int i = 0; i < partData.rendererParts.Length; ++i)
            {
                var rendererData = partData.rendererParts[i];

                SkinnedMeshRenderer renderer = nodeBehaviour.GetSMRenderer(rendererData.rendererName);
                if (renderer != null)
                {
                    SkinnedMeshRenderer smRenderer = renderer;
                    smRenderer.rootBone = nodeBehaviour.GetBoneTransform(rendererData.rootBoneName);
                    smRenderer.bones = nodeBehaviour.GetBoneTransformByNames(rendererData.boneNames);
                    smRenderer.sharedMesh = rendererData.mesh;
                    smRenderer.sharedMaterials = rendererData.materials;

                    partInstance.renderers[i] = smRenderer;
                }
                else
                {
                    LogUtil.Error("AvatarUtil", $"AvatarUtil::AssembleAvatarPart->nodeData not found.rendererNodeName={rendererData.rendererName}");
                }
            }

            return partInstance;
        }

        public static void DisassemblePart(AvatarPartInstance partInstance)
        {
            foreach (var go in partInstance.gameObjects)
            {
#if UNITY_EDITOR
                if(Application.isPlaying)
                {
                    GameObject.Destroy(go);
                }else
                {
                    GameObject.DestroyImmediate(go);
                }
#else
                GameObject.Destroy(go);
#endif
            }
            foreach (var smr in partInstance.renderers)
            {
                if (smr != null)
                {
                    smr.rootBone = null;
                    smr.bones = new Transform[0];
                    smr.sharedMesh = null;
                    smr.sharedMaterials = new Material[0];
                }
            }
        }
    }
}
