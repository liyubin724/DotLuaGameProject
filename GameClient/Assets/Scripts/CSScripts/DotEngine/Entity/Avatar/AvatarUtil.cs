using DotEngine.Entity.Node;
using DotEngine.Log;
using UnityEngine;

namespace DotEngine.Entity.Avatar
{
    public static class AvatarUtil
    {
        public static AvatarPartInstance AssembleAvatarPart(NodeBehaviour nodeBehaviour, AvatarPartData partData)
        {
            AvatarPartInstance partInstance = new AvatarPartInstance();
            partInstance.partType = partData.partType;
            partInstance.gameObjects = new GameObject[partData.prefabParts.Length];
            for (int i = 0; i < partData.prefabParts.Length; ++i)
            {
                var prefabData = partData.prefabParts[i];

                GameObject bindGameObject = GameObject.Instantiate(prefabData.prefabGO);
                NodeData bindNodeData = nodeBehaviour.GetBindNode(prefabData.bindNodeName);
                bindGameObject.transform.SetParent(bindNodeData.transform, false);

                partInstance.gameObjects[i] = bindGameObject;
            }

            partInstance.renderers = new SkinnedMeshRenderer[partData.rendererParts.Length];
            for (int i = 0; i < partData.rendererParts.Length; ++i)
            {
                var rendererData = partData.rendererParts[i];

                NodeData nodeData = nodeBehaviour.GetSMRendererNode(rendererData.rendererNodeName);
                if (nodeData != null)
                {
                    SkinnedMeshRenderer smRenderer = nodeData.renderer;
                    smRenderer.rootBone = nodeBehaviour.GetBoneNode(rendererData.rootBoneName).transform;
                    smRenderer.bones = nodeBehaviour.GetBoneTransformByNames(rendererData.boneNames);
                    smRenderer.sharedMesh = rendererData.mesh;
                    smRenderer.sharedMaterials = rendererData.materials;

                    partInstance.renderers[i] = smRenderer;
                }
                else
                {
                    LogUtil.LogError("AvatarUtil", $"AvatarUtil::AssembleAvatarPart->nodeData not found.rendererNodeName={rendererData.rendererNodeName}");
                }
            }

            return partInstance;
        }

        public static void DisassembleAvatarPart(AvatarPartInstance partInstance, bool isInEditorMode = false)
        {
            foreach (var go in partInstance.gameObjects)
            {
                if (isInEditorMode)
                {
                    GameObject.DestroyImmediate(go);
                }
                else
                {
                    GameObject.Destroy(go);
                }
            }
            foreach (var smr in partInstance.renderers)
            {
                if (smr != null)
                {
                    smr.sharedMaterials = new Material[0];
                    smr.rootBone = null;
                    smr.sharedMesh = null;
                    smr.bones = new Transform[0];
                }
            }
        }
    }
}
