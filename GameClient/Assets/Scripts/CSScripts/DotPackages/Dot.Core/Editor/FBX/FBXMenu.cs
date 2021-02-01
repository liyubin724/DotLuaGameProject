using DotEditor.Utilities;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.FBX
{
    public class FBXMenu
    {
        [MenuItem("Game/Animation Clip/compress clips")]
        public static void CompressClip()
        {
            AnimationClip[] selectedClips = Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);
            if (selectedClips == null || selectedClips.Length == 0)
            {
                return;
            }

            var clipInfos = (from clip in selectedClips
                             let clipAssetPath = AssetDatabase.GetAssetPath(clip)
                             select AnimationClipCompress.CreateCompressedClip(clip,4)
                             ).ToArray();

            SelectionUtility.ActiveObjects(clipInfos);
        }

        [MenuItem("Game/Animation Clip/extract clips from fbx")]
        public static AnimationClip[] ExtractClipsFromFBX()
        {
            GameObject[] gameObjects = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
            if (gameObjects == null || gameObjects.Length == 0)
            {
                return null;
            }

            var clips = (from go in gameObjects
                         let assetPath = AssetDatabase.GetAssetPath(go)
                         where Path.GetExtension(assetPath).ToLower() == ".fbx"
                         let extractClips = AnimationClipExtract.ExtractClipFromFBX(assetPath, null)
                         from clip in extractClips
                         select clip).ToList();

            SelectionUtility.ActiveObjects(clips.ToArray());

            return clips.ToArray();
        }

        [MenuItem("Game/Animation Clip/extract and compress clips from fbx")]
        public static void ExtractAnCompressClipsFromFBX()
        {
            AnimationClip[] extractClips = ExtractClipsFromFBX();
            if (extractClips != null && extractClips.Length > 0)
            {
                var clipInfos = (from clip in extractClips
                                 let clipAssetPath = AssetDatabase.GetAssetPath(clip)
                                 select AnimationClipCompress.CreateCompressedClip(clip, 4)
                             ).ToArray();

                SelectionUtility.ActiveObjects(clipInfos);
            }
        }

        //[MenuItem("Test/Model Undo Optimize")]
        //public static void Undooptimize()
        //{
        //    UnityObject uObj = Selection.activeObject;
        //    string assetPath = AssetDatabase.GetAssetPath(uObj);

        //    UnoptimizeGameObjects(assetPath);
        //}

        //[MenuItem("Test/Model Optimize")]
        //public static void Optimize()
        //{
        //    UnityObject uObj = Selection.activeObject;
        //    string assetPath = AssetDatabase.GetAssetPath(uObj);

        //    OptimizeGameObjects(assetPath, new string[] { "Bip001 R Toe0" });
        //}
    }
}
