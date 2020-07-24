using System.Linq;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 加载资源时返回数据结构，可用于检视加载情况
    /// 注意：除了使用侦听外，还可以通过<see cref="AssetHandler"/>来检视加载情况
    /// </summary>
    public class AssetHandler
    {
        /// <summary>
        /// 资源标记的标签，如果资源不是以标签的形式加载的话，则返回null
        /// </summary>
        public string Label { get; private set; }
        /// <summary>
        /// 加载的所有资源地址
        /// </summary>
        public string[] Addresses { get; private set; }
        /// <summary>
        /// 资源地址，如果加载的是单个资源，可能得到此资源的地址
        /// </summary>
        public string Address
        {
            get
            {
                if(Addresses!=null && Addresses.Length>0)
                {
                    return Addresses[0];
                }
                return null;
            }
        }
        /// <summary>
        /// 加载到的资源或者资源的实例，在使用时请提前使用<see cref="IsDone"/>来检查是否加载完毕
        /// </summary>
        public UnityObject[] UObjects { get; internal set; }
        /// <summary>
        /// 加载到的资源或者资源的实例
        /// </summary>
        public UnityObject UObject
        {
            get
            {
                if(UObjects!=null && UObjects.Length>0)
                {
                    return UObjects[0];
                }
                return null;
            }
        }

        /// <summary>
        ///  加载的每个资源的进度
        /// </summary>
        public float[] Progresses { get; internal set; }
        /// <summary>
        /// 资源加载进度
        /// </summary>
        public float Progress { 
            get
            {
                if(Progresses!=null && Progresses.Length>0)
                {
                    return Progresses[0];
                }
                return 0.0f;
            }
        }
        /// <summary>
        /// 资源加载总体进度
        /// </summary>
        public float TotalProgress
        {
            get
            {
                if(Progresses!=null && Progresses.Length>0)
                {
                    return Progresses.Sum() / Progresses.Length;
                }
                return 0.0f;
            }
        }
        /// <summary>
        /// 自定义参数
        /// </summary>
        public SystemObject UserData { get; private set; }

        /// <summary>
        /// 资源是否加载完毕
        /// </summary>
        public bool IsDone { get; internal set; } = false;

        /// <summary>
        /// 构造函数，仅供包内使用，外部无法创建
        /// </summary>
        /// <param name="label">资源标签</param>
        /// <param name="addresses">所有资源地址</param>
        /// <param name="userData">自定义参数</param>
        internal AssetHandler(string label,string[] addresses,SystemObject userData)
        {
            Label = label;
            Addresses = addresses;
            UserData = userData;

            Progresses = new float[addresses.Length];
            UObjects = new UnityObject[addresses.Length];
        }

        /// <summary>
        /// 取消资源加载，仅供包内部使用
        /// </summary>
        /// <param name="isInstance">是否需要实例化</param>
        /// <param name="destroyIfIsInstnace">是否销毁已经实例化的对象</param>
        internal void DoCancel(bool isInstance,bool destroyIfIsInstnace)
        {
            if(isInstance && destroyIfIsInstnace)
            {
                for(int  i = 0;i<UObjects.Length;++i)
                {
                    UnityObject uObj = UObjects[i];
                    if(uObj!=null)
                    {
                        UnityObject.Destroy(uObj);
                        UObjects[i] = null;
                    }
                }
            }
            IsDone = false;
            UObjects = null;
            Label = null;
            Addresses = null;
            Progresses = null;
            UserData = null;
        }
    }
}
