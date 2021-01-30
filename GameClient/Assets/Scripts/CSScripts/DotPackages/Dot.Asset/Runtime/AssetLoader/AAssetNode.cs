using DotEngine.Pool;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 加载中及缓存到的资源结点
    /// </summary>
    public abstract class AAssetNode : IObjectPoolItem
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; private set; }
        /// <summary>
        /// 是否设定此资源永不清理
        /// </summary>
        public bool IsNeverDestroy { get; set; }

        /// <summary>
        /// 资源被重复使用的次数，如果次数大于0，则会不执行清理
        /// </summary>
        protected int refCount = 0;
        protected internal void RetainRef() => ++refCount;
        protected internal void ReleaseRef() => --refCount;

        protected internal void InitNode(string path)
        {
            AssetPath = path;
        }
        /// <summary>
        /// 获取加载到的资源
        /// </summary>
        /// <returns></returns>
        protected internal abstract UnityObject GetAsset();
        /// <summary>
        /// 获取加载到的资源的实例
        /// </summary>
        /// <returns></returns>
        protected internal abstract UnityObject GetInstance();
        protected internal abstract UnityObject GetInstance(UnityObject uObj);
        /// <summary>
        /// 判断是否还存活，如果不在存活，将会被清理
        /// </summary>
        /// <returns></returns>
        protected internal abstract bool IsAlive();
        /// <summary>
        /// 资源是否加载结束
        /// </summary>
        /// <returns></returns>
        protected internal abstract bool IsDone();
        protected internal abstract void Unload();

        public void OnGet()
        {
        }

        public virtual void OnRelease()
        {
            AssetPath = null;
            IsNeverDestroy = false;
            refCount = 0;
        }
    }
}
