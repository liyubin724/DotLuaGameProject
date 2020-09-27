using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.U2D;
using DotEditor.Utilities;

namespace DotEditor.Asset
{
    public class TemporaryRedundancyChecker
    {
        [MenuItem("Game/Asset/TR Checker")]
        public static void TRChecker()
        {
            new TemporaryRedundancyChecker().Check();
        }

        public void Check()
        {
            FindAssetBundleAsset((title, message, progress) =>
            {
                EditorUtility.DisplayProgressBar(title, message, progress);
            });
            FindSpriteInAtlas((title, message, progress) =>
            {
                EditorUtility.DisplayProgressBar(title, message, progress);
            });
            FindBundleDepends((title, message, progress) =>
            {
                EditorUtility.DisplayProgressBar(title, message, progress);
            });
            FindAssetUsedCount((title, message, progress) =>
            {
                EditorUtility.DisplayProgressBar(title, message, progress);
            });
            EditorUtility.ClearProgressBar();

            Dictionary<string, int> repeatCountDic = GetRepeatUsedAssets();
            List<string> assetKeys = new List<string>(repeatCountDic.Keys);
            using(StreamWriter sw = new StreamWriter(new FileStream("D:/rc-log.txt", FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine("冗余资源数量：" + repeatCountDic.Count);

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("重复次数最多的前20个资源：");
                assetKeys.Sort((item1, item2) =>
                {
                    return repeatCountDic[item2].CompareTo(repeatCountDic[item1]);
                });

                for(int i =0;i<20 && i<assetKeys.Count;++i)
                {
                    sw.WriteLine($"{assetKeys[i]}\t\t\t{repeatCountDic[assetKeys[i]]}");
                }

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("按文件扩展名进行分类统计：");

                Dictionary<string, int> extensionRepeatCountDic = new Dictionary<string, int>();
                foreach(var key in assetKeys)
                {
                    string extension = Path.GetExtension(key).ToLower();
                    if(extensionRepeatCountDic.ContainsKey(extension))
                    {
                        extensionRepeatCountDic[extension]++;
                    }
                    else
                    {
                        extensionRepeatCountDic.Add(extension, 1);
                    }
                }
                List<string> extensions = extensionRepeatCountDic.Keys.ToList();
                extensions.Sort((item1, item2) =>
                {
                    return extensionRepeatCountDic[item2].CompareTo(extensionRepeatCountDic[item1]);
                });

                foreach(var extension in extensions)
                {
                    sw.WriteLine($"{extension}\t\t\t{extensionRepeatCountDic[extension]}");
                }

                sw.Flush();
                sw.Close();
            }
        }

        private List<string> m_BundleAssetPathList = new List<string>();
        public void FindAssetBundleAsset(Action<string,string,float> progressAction)
        {
            m_BundleAssetPathList.Clear();
            AssetDatabase.RemoveUnusedAssetBundleNames();

            string[] abNames = AssetDatabase.GetAllAssetBundleNames();
            if(abNames!=null && abNames.Length>0)
            {
                for(int i = 0;i<abNames.Length;++i)
                {
                    progressAction?.Invoke("Find Assets", abNames[i], i / (float)abNames.Length);
                    string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abNames[i]);
                    if(assetPaths!=null && assetPaths.Length>0)
                    {
                        m_BundleAssetPathList.AddRange(assetPaths);
                    }
                }
            }
        }

        private Dictionary<string, string> m_SpriteInAtlasDic = new Dictionary<string, string>();
        public void FindSpriteInAtlas(Action<string, string, float> progressAction)
        {
            List<string> atlasPaths = (from path in m_BundleAssetPathList
                                       where Path.GetExtension(path).ToLower() == ".spriteatlas"
                                       select path).ToList();

            int m_FindIndex = 0;
            atlasPaths.ForEach((atlasPath) =>
            {
                progressAction?.Invoke("Find Sprite In Atlas",atlasPath, m_FindIndex / (float)m_BundleAssetPathList.Count);
                m_FindIndex++;

                SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
                if (atlas != null)
                {
                    string[] spriteInAtlas = SpriteAtlasUtility.GetDependAssets(atlas);
                    Array.ForEach(spriteInAtlas, spritePath =>
                    {
                        m_SpriteInAtlasDic.Add(spritePath, atlasPath);
                    });
                }
            });
        }

        private Dictionary<string, List<string>> m_BundleDependAssetDic = new Dictionary<string, List<string>>();
        private void FindBundleDepends(Action<string, string, float> progressAction)
        {
            int m_FindIndex = 0;
            m_BundleAssetPathList.ForEach((path) =>
            {
                if (!(Path.GetExtension(path).ToLower() == ".spriteatlas"))
                {
                    progressAction?.Invoke("Find Depends",path, m_FindIndex / (float)m_BundleAssetPathList.Count);
                    m_FindIndex++;

                    List<string> depends = new List<string>();

                    string[] dependAssets = AssetDatabaseUtility.GetDependencies(path, new string[] { ".cs",".txt" });
                    foreach(var dependAsset in dependAssets)
                    {
                        if (dependAsset != path  && m_BundleAssetPathList.IndexOf(dependAsset) < 0 &&
                            depends.IndexOf(dependAsset) < 0 && !m_SpriteInAtlasDic.ContainsKey(dependAsset))
                        {
                            depends.Add(dependAsset);
                        }
                    }
                    m_BundleDependAssetDic.Add(path, depends);
                }
            });
        }

        private Dictionary<string, int> m_AssetUsedCountDic = new Dictionary<string, int>();
        private void FindAssetUsedCount(Action<string, string, float> progressAction)
        {
            int m_FindIndex = 0;
            foreach (var kvp in m_BundleDependAssetDic)
            {
                progressAction?.Invoke("Find Asset Used Count", kvp.Key, m_FindIndex / (float)m_BundleAssetPathList.Count);
                m_FindIndex++;

                foreach (var path in kvp.Value)
                {
                    if (m_AssetUsedCountDic.ContainsKey(path))
                    {
                        m_AssetUsedCountDic[path]++;
                    }
                    else
                    {
                        m_AssetUsedCountDic.Add(path, 1);
                    }
                }
            }
        }

        private Dictionary<string,int> GetRepeatUsedAssets()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var kvp in m_AssetUsedCountDic)
            {
                if (kvp.Value > 1)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            return result;
        }
    }
}
