using System.Reflection;
using UnityEngine;

namespace DotEditor.Extensions
{
    public static class AssemblyEx
    {
        /// <summary>
        /// 从程序集中获取图片资源，并转换为Texture2D对象
        /// </summary>
        /// <param name="assembly">包含有指定图片的程序集</param>
        /// <param name="filePath">图片资源的路径，取决于程序集的包名及目录布局</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(this Assembly assembly,string filePath)
        {
            Texture2D texture = new Texture2D(1, 1);
            using(var stream = assembly.GetManifestResourceStream(filePath))
            {
                if(stream == null)
                {
                    Debug.LogError($"AssemblyEx::LoadTexture->GetManifestResourceStream failed. assemblyName = {assembly.FullName}, path = {filePath}");
                    return null;
                }

                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                if(!texture.LoadImage(buffer))
                {
                    Debug.LogError($"AssemblyEx::LoadTexture->LoadImage failed. assemblyName = {assembly.FullName}, path = {filePath}");
                    return null;
                }
            }

            return texture;
        }
    }
}
