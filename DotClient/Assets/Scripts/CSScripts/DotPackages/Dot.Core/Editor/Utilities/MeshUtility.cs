using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Utilities
{
    public static class MeshUtility
    {

        public static Mesh CopyMeshTo(Mesh sourceMesh,string targetPath,ModelImporterMeshCompression compression = ModelImporterMeshCompression.Off)
        {
            Mesh targetMesh = AssetDatabase.LoadAssetAtPath<Mesh>(targetPath);
            if(targetMesh!=null)
            {
                AssetDatabase.DeleteAsset(targetPath);
            }

            targetMesh = UnityObject.Instantiate<Mesh>(sourceMesh);
            if(compression!= ModelImporterMeshCompression.Off)
            {
                UnityEditor.MeshUtility.SetMeshCompression(targetMesh, compression);
            }
            UnityEditor.MeshUtility.Optimize(targetMesh);

            AssetDatabase.CreateAsset(targetMesh, targetPath);
            AssetDatabase.ImportAsset(targetPath);

            return targetMesh;
        }
    }
}
