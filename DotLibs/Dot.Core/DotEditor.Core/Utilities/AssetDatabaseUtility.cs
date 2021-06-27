using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Utilities
{
    public static class AssetDatabaseUtility
    {
        public static bool IsFolder(UnityObject uObject)
        {
            return uObject.GetType() == typeof(DefaultAsset) && ProjectWindowUtil.IsFolder(uObject.GetInstanceID());
        }

        #region FindAssets

        /// <summary>
        /// 查找所有的场景资源
        /// </summary>
        /// <returns></returns>
        public static string[] FindScenes()
        {
            return FindAssets(null, null, typeof(Scene), null, null);
        }

        /// <summary>
        /// 在指定的目标中筛选场景资源
        /// </summary>
        /// <param name="searchFolders">设定筛选的目录</param>
        /// <returns></returns>
        public static string[] FindScenesInFolders(string name, string[] searchFolders)
        {
            return FindAssets(name, null, typeof(Scene), null, searchFolders);
        }

        /// <summary>
        /// 在给定的目录(folderPath)中查找指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static T[] FindInstancesInFolder<T>(string folderPath) where T : UnityEngine.Object
        {
            string[] assetPaths = FindAssetInFolder<T>(folderPath);
            if (assetPaths == null || assetPaths.Length == 0)
            {
                return null;
            }
            T[] result = new T[assetPaths.Length];
            for (int i = 0; i < assetPaths.Length; ++i)
            {
                result[i] = AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
            }
            return result;
        }

        /// <summary>
        /// 在给定的目录(folderPath)中查找指定类型的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static string[] FindAssetInFolder<T>(string folderPath)
        {
            return FindAssets(null, null, typeof(T), null, string.IsNullOrEmpty(folderPath) ? null : new string[] { folderPath });
        }

        public static T[] FindInstances<T>() where T : UnityEngine.Object
        {
            string[] assetPaths = FindAssets<T>();
            if (assetPaths == null || assetPaths.Length == 0)
            {
                return new T[0];
            }
            T[] result = new T[assetPaths.Length];
            for (int i = 0; i < assetPaths.Length; ++i)
            {
                result[i] = AssetDatabase.LoadAssetAtPath<T>(assetPaths[i]);
            }
            return result;
        }

        /// <summary>
        /// 查找指定类型的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] FindAssets<T>() where T : UnityEngine.Object
        {
            return FindAssets(null, null, typeof(T), null, null);
        }

        /// <summary>
        /// 对Project中的资源按指定的规则进行筛选
        /// </summary>
        /// <param name="name">根据资源的名称进行资源的筛选</param>
        /// <param name="label">根据设定的标签label进行资源的筛选</param>
        /// <param name="type">根据设定的资源类型进行资源的筛选</param>
        /// <param name="bundleName">根据设定的Bundle的名称进行资源筛选</param>
        /// <param name="searchFolders">设定筛选的目录</param>
        /// <returns>返回资源的路径</returns>
        private static string[] FindAssets(string name, string label, Type type, string bundleName, string[] searchFolders)
        {
            StringBuilder searchPattern = new StringBuilder();
            if (!string.IsNullOrEmpty(name))
            {
                searchPattern.Append(name);
            }
            if (!string.IsNullOrEmpty(label))
            {
                if (searchPattern.Length > 0)
                {
                    searchPattern.Append(" ");
                }
                searchPattern.Append($"l:{label}");
            }
            if (type != null)
            {
                if (searchPattern.Length > 0)
                {
                    searchPattern.Append(" ");
                }
                searchPattern.Append($"t:{type.Name}");
            }

            if (!string.IsNullOrEmpty(bundleName))
            {
                if (searchPattern.Length > 0)
                {
                    searchPattern.Append(" ");
                }
                searchPattern.Append($"b:{bundleName}");
            }

            string[] guids = AssetDatabase.FindAssets(searchPattern.ToString(), searchFolders);
            return GetAssetPathByGUID(guids);
        }

        /// <summary>
        /// 将guid转换成资源路径
        /// 由于使用AssetDatabase.FindAssets得到的资源为guid,为方便使用需要转换一下
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        private static string[] GetAssetPathByGUID(string[] guids)
        {
            if (guids == null)
            {
                return new string[0];
            }

            string[] paths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }
            return paths;
        }

        #endregion

        public static bool HasAssetAtPath(string assetPath)
        {
            if(string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            return AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath) != null;
        }

        public static bool IsAssetAtPath<T>(string assetPath) where T:UnityObject
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            return AssetDatabase.LoadAssetAtPath<T>(assetPath) != null;
        }

        public static T CreateAsset<T>() where T: ScriptableObject
        {
            string selectedDir = SelectionUtility.GetSelectionDir();
            if(string.IsNullOrEmpty(selectedDir))
            {
                selectedDir = "Assets";
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{selectedDir}/{typeof(T).Name.ToLower()}.asset");
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            AssetDatabase.ImportAsset(assetPath);
            SelectionUtility.ActiveObject(asset);
            return asset;
        }

        /// <summary>
        /// 在Project中创建指定类型的资源
        /// 只能创建ScriptableObject类型的资源，对于给定的名称，如果目录中已经存在相同名称的资源，会进行重命名
        /// 创建资源时可以指定资源存储的目录 (assetFolder基于Unity的Assets目录)
        /// </summary>
        /// <typeparam name="T">资源的类型</typeparam>
        /// <param name="fileName">资源名称</param>
        /// <param name="assetFolder">资源存储目录，默认为空</param>
        /// <returns></returns>
        public static T CreateAsset<T>(string fileName, string assetFolder = "") where T : ScriptableObject
        {
            if (fileName.Contains("/"))
                throw new ArgumentException("Base name should not contain slashes");
            if (!string.IsNullOrEmpty(assetFolder) && !assetFolder.StartsWith("Assets"))
            {
                throw new ArgumentException("Asset Folder should be start with Assets");
            }

            string folderPath = string.Empty;
            if (string.IsNullOrEmpty(assetFolder))
            {
                string[] folders = SelectionUtility.GetSelectionDirs();
                if (folders == null || folders.Length == 0)
                {
                    folderPath = "Assets";
                }
                else
                {
                    folderPath = folders[0];
                }
            }
            else
            {
                folderPath = assetFolder;
            }

            var asset = ScriptableObject.CreateInstance<T>();
            string assetPath = $"{folderPath}/{fileName}.asset";
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.ImportAsset(assetPath);
            return asset;
        }
        /// <summary>
        /// 查找指定的资源依赖的所有资源
        /// 通过设定ignoreExt的值可以忽略掉指定文件后缀的资源
        /// 默认情况下后缀的检查，以小写字母进行。如string[] ignoreExt = new string[]{".cs"}
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="ingoreExt"></param>
        /// <returns></returns>
        public static string[] GetDependencies(string assetPath, string[] ignoreExt = null)
        {
            return GetAssetDependencies(assetPath, true, ignoreExt);
        }

        /// <summary>
        /// 查找指定的资源直接依赖的资源
        /// 通过设定ignoreExt的值可以忽略掉指定文件后缀的资源
        /// 默认情况下后缀的检查，以小写字母进行。如string[] ignoreExt = new string[]{".cs"}
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="ingoreExt"></param>
        /// <returns></returns>
        public static string[] GetDirectlyDependencies(string assetPath, string[] ignoreExt = null)
        {
            return GetAssetDependencies(assetPath, false, ignoreExt);
        }

        private static string[] GetAssetDependencies(string assetPath, bool isRecursive, string[] ignoreExt)
        {
            string[] assetPaths = AssetDatabase.GetDependencies(assetPath, isRecursive);
            if (ignoreExt == null || ignoreExt.Length == 0)
            {
                return assetPaths;
            }

            return (from path in assetPaths
                    let ext = Path.GetExtension(path).ToLower()
                    where Array.IndexOf(ignoreExt, ext) < 0
                    select path).ToArray();
        }

        public static long GetTextureStorageSize(Texture texture)
        {
            if (texture == null)
                return 0;

            var TextureUtilType = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
            MethodInfo methodInfo = TextureUtilType.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Public);

            return (long)methodInfo.Invoke(null, new SystemObject[] { texture });
        }

        public static long GetTextureStorageSize(string assetPath)
        {
            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
            return GetTextureStorageSize(texture);
        }

        public static long GetAssetRuntimeMemorySize(UnityObject uObj)
        {
            if(uObj == null)
            {
                return 0;
            }
            return Profiler.GetRuntimeMemorySizeLong(uObj);
        }

        public static long GetAssetRuntimeMemorySize(string assetPath)
        {
            UnityObject uObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
            return Profiler.GetRuntimeMemorySizeLong(uObj);
        }
    }
}
