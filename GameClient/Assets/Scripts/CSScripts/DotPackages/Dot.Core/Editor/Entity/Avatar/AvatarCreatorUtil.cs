using DotEditor.Entity.Node;
using DotEditor.Utilities;
using DotEngine.Entity.Avatar;
using DotEngine.Entity.Node;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;
using static DotEngine.Entity.Avatar.AvatarPartData;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Entity.Avatar
{
    public static class AvatarCreatorUtil
    {
        public static GameObject CreateSkeleton(AvatarSkeletonCreatorData data)
        {
            if (data == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->the data is null");
                return null;
            }

            if (data.fbx == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->The fbx is null");
                return null;
            }

            PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(data.fbx);
            if (assetType != PrefabAssetType.Model)
            {
                Debug.LogError($"AvatarCreatorUtil::CreateSkeleton->The fbx is not a model.type = {assetType}");
                return null;
            }

            if (string.IsNullOrEmpty(data.outputFolder))
            {
                Debug.LogError("AvatarCreatorUtil::CreateSkeleton->The outputFolder is empty");
                return null;
            }

            string outputDiskFolder = PathUtility.GetDiskPath(data.outputFolder);
            if (!Directory.Exists(outputDiskFolder))
            {
                Directory.CreateDirectory(outputDiskFolder);
            }

            string skeletonPrefabAssetPath = data.GetSkeletonPrefabPath();
            GameObject cachedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPrefabAssetPath);
            NodeBehaviour cachedNodeBehaviour = cachedPrefab?.GetComponent<NodeBehaviour>();

            GameObject instanceGameObject = GameObject.Instantiate<GameObject>(data.fbx);
            NodeBehaviour instanceNodeBehaviour = instanceGameObject.AddComponent<NodeBehaviour>();

            instanceNodeBehaviour.FindBoneNodes();
            instanceNodeBehaviour.FindSMRendererNodes();
            if (cachedNodeBehaviour != null)
            {
                instanceNodeBehaviour.CopyFrom(cachedNodeBehaviour);
            }

            if (instanceNodeBehaviour.smRendererNodes != null && instanceNodeBehaviour.smRendererNodes.Length > 0)
            {
                foreach (var nodeData in instanceNodeBehaviour.smRendererNodes)
                {
                    SkinnedMeshRenderer smr = nodeData.renderer;
                    if (smr != null)
                    {
                        smr.sharedMaterials = new Material[0];
                        smr.rootBone = null;
                        smr.sharedMesh = null;
                        smr.bones = new Transform[0];
                    }
                }
            }

            UnityEditor.PrefabUtility.SaveAsPrefabAsset(instanceGameObject, skeletonPrefabAssetPath);
            GameObject.DestroyImmediate(instanceGameObject);

            AssetDatabase.ImportAsset(skeletonPrefabAssetPath);

            return AssetDatabase.LoadAssetAtPath<GameObject>(skeletonPrefabAssetPath);
        }

        public static AvatarPartData CreatePart(string outputFolder, AvatarPartCreatorData data)
        {
            if (data == null)
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->the data is null");
                return null;
            }

            if (string.IsNullOrEmpty(outputFolder))
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->The outputFolder is empty");
                return null;
            }

            if (string.IsNullOrEmpty(data.name))
            {
                Debug.LogError("AvatarCreatorUtil::CreatePart->The name is empty");
                return null;
            }

            string partAssetPath = data.GetPartAssetPath(outputFolder);

            AvatarPartData partData = ScriptableObject.CreateInstance<AvatarPartData>();
            partData.name = data.name;
            partData.partName = data.partName;

            List<PrefabPartData> prefabPartDatas = new List<PrefabPartData>();
            foreach (var prefabCreatorData in data.prefabDatas)
            {
                if (string.IsNullOrEmpty(prefabCreatorData.bindNodeName))
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The bindNodeName is empty");
                    return null;
                }

                if (prefabCreatorData.bindPrefab == null)
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The bindPrefab is null");
                    return null;
                }

                PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(prefabCreatorData.bindPrefab);
                if (assetType != PrefabAssetType.Regular)
                {
                    Debug.LogError($"AvatarCreatorUtil::CreatePart->The bindPrefab is not a Prefab.type = {assetType}");
                    return null;
                }

                PrefabPartData prefabPartData = new PrefabPartData();
                prefabPartData.bindName = prefabCreatorData.bindNodeName;
                prefabPartData.prefabGO = prefabCreatorData.bindPrefab;
                prefabPartDatas.Add(prefabPartData);
            }
            partData.prefabParts = prefabPartDatas.ToArray();

            List<RendererPartData> rendererPartDatas = new List<RendererPartData>();
            foreach (var rendererCreatorData in data.rendererDatas)
            {
                if (rendererCreatorData.fbx == null)
                {
                    Debug.LogError("AvatarCreatorUtil::CreatePart->The fbx is null");
                    return null;
                }

                PrefabAssetType assetType = UnityEditor.PrefabUtility.GetPrefabAssetType(rendererCreatorData.fbx);
                if (assetType != PrefabAssetType.Model)
                {
                    Debug.LogError($"AvatarCreatorUtil::CreatePart->The fbx is not a model.type = {assetType}");
                    return null;
                }

                SkinnedMeshRenderer[] renderers = rendererCreatorData.fbx.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach (var renderer in renderers)
                {
                    RendererPartData rendererPartData = new RendererPartData();
                    rendererPartData.rendererName = renderer.name;
                    rendererPartData.rootBoneName = renderer.rootBone.name;
                    rendererPartData.boneNames = (from bone in renderer.bones select bone.name).ToArray();
                    rendererPartData.materials = renderer.sharedMaterials;

                    Mesh mesh = renderer.sharedMesh;
                    string meshAssetPath = $"{outputFolder}/{mesh.name}_mesh.asset";
                    if (rendererCreatorData.IsCopyMesh)
                    {
                        mesh = Utilities.MeshUtility.CopyMeshTo(mesh, meshAssetPath);
                    }
                    else
                    {
                        if (AssetDatabase.LoadAssetAtPath<UnityObject>(meshAssetPath) != null)
                        {
                            AssetDatabase.DeleteAsset(meshAssetPath);
                        }
                    }
                    rendererPartData.mesh = mesh;

                    rendererPartDatas.Add(rendererPartData);
                }
            }
            partData.rendererParts = rendererPartDatas.ToArray();


            AssetDatabase.CreateAsset(partData, partAssetPath);
            AssetDatabase.ImportAsset(partAssetPath);

            return partData;
        }
    }
}
