using DotEngine.Extensions;
using DotEngine.Log;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public delegate void PoolPreloadComplete(string assetPath);

    /// <summary>
    /// 用于创建缓存池的模板的类型
    /// </summary>
    public enum UGOTemplateType
    {
        Prefab = 0,//使用Prefab做为缓存池模型
        PrefabInstance,//使用Prefab实例化后的对象做为模板
        RuntimeInstance,//运行时创建的对象做模板
    }

    /// <summary>
    /// 以GameObject为对象的缓存池
    /// </summary>
    public class UGOPool
    {
        /// <summary>
        /// 资源标识，一般使用资源的路径或者指定一个唯一的名称
        /// </summary>
        public string AssetPath { get; private set; }

        private UGOTemplateType templateType;
        // 缓存池中使用的GameObject的模板
        private GameObject templateGameObject;
        // 用于缓存对象的结点
        private Transform poolTransform;
        // 缓存池预加载完后回调
        private PoolPreloadComplete preloadCompleteCallback = null;
        //预加载的GameObject数量
        private int preloadTotalAmount = 0;
        //每帧加载的数量，可以防止一次大量的预加载造成卡顿，一般可以在Loading中提前处理
        private int preloadOnceAmount = 0;

        private bool isCullEnable = false;
        //进行清理时，一次性清理的数量，为小于等于0表示一次性清理所有的
        private int cullOnceAmount = 0;
        //两次清理执行时间间隔,以秒为单位
        private float cullDelayTime = 60.0f;
        private float preCullTime = 0.0f;

        //可以缓存的对象的数量
        private int cachedMaxAmount = int.MaxValue;

        // 空闲的GameObject对象栈
        private Queue<GameObject> unusedItemQueue = new Queue<GameObject>();

        /// <summary>
        /// 从缓存池中获取的正在使用的对象
        /// 为了上层方便对GameObject管理，采用弱引用的方式存储，
        /// 可以保证即使是上层删除了GameObject也不会对整体造成影响
        /// </summary>
        private List<WeakReference<GameObject>> usedItemList = new List<WeakReference<GameObject>>();

        internal UGOPool(
            Transform parentTransform,
            string assetPath,
            UGOTemplateType templateType,
            GameObject templateGObj)
        {
            AssetPath = assetPath;
            this.templateType = templateType;
            templateGameObject = templateGObj;
            if (UGOPoolUtill.IsDebug)
            {
                poolTransform = new GameObject(assetPath).transform;
                poolTransform.SetParent(parentTransform, false);
            }
            else
            {
                poolTransform = parentTransform;
            }

            if (templateType != UGOTemplateType.Prefab)
            {
                templateGameObject.SetActive(false);
                templateGameObject.transform.SetParent(poolTransform, false);
            }
        }

        internal void DoUpdate(float deltaTime)
        {
            if (preloadTotalAmount > 0)
            {
                PreloadItem();
                return;
            }

            if (preloadCompleteCallback != null)
            {
                preloadCompleteCallback.Invoke(AssetPath);
                preloadCompleteCallback = null;
                return;
            }

            //cull
            if (isCullEnable && cullDelayTime > 0)
            {
                preCullTime += deltaTime;
                if (preCullTime >= cullDelayTime)
                {
                    Cull();
                    preCullTime = 0.0f;
                }
            }
        }

        #region Preload
        public void SetPreload(int totalAmount, int onceAmount, PoolPreloadComplete callback = null)
        {
            preloadTotalAmount = totalAmount;
            preloadOnceAmount = onceAmount;

            preloadCompleteCallback = callback;
        }

        private void PreloadItem()
        {
            int amount = preloadTotalAmount;
            if (preloadOnceAmount > 0)
            {
                amount = Mathf.Min(preloadOnceAmount, amount);
            }
            for (int i = 0; i < amount; ++i)
            {
                GameObject instance = CreateItem();
                instance.transform.SetParent(poolTransform, false);
                instance.SetActive(false);

                unusedItemQueue.Enqueue(instance);
            }

            preloadTotalAmount -= amount;
        }
        #endregion

        #region Cull
        public void SetCull(int onceAmount, float delayTime)
        {
            isCullEnable = true;
            cullOnceAmount = onceAmount;
            cullDelayTime = delayTime;
        }

        private void Cull()
        {
            if (UGOPoolUtill.IsDebug)
            {
                for (int i = usedItemList.Count - 1; i >= 0; --i)
                {
                    if (usedItemList[i].TryGetTarget(out GameObject gObj) && gObj.IsNull())
                    {
                        usedItemList.RemoveAt(i);
                    }
                }
            }

            if (unusedItemQueue.Count <= cachedMaxAmount)
            {
                return;
            }

            int amount = unusedItemQueue.Count - cachedMaxAmount;
            if (cullOnceAmount > 0)
            {
                amount = Mathf.Min(amount, cullOnceAmount);
            }
            for (int i = 0; i < amount; ++i)
            {
                DestroyItem(unusedItemQueue.Dequeue());
            }
        }
        #endregion

        #region cached max count
        public void SetCachedMaxCount(int count)
        {
            cachedMaxAmount = count;
        }
        #endregion

        #region GetItem
        /// <summary>
        /// 从缓存池中得到一个GameObject对象
        /// </summary>
        /// <param name="isActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public GameObject GetItem(bool isActive = true)
        {
            GameObject item;
            if (unusedItemQueue.Count > 0)
            {
                item = unusedItemQueue.Dequeue();
            }
            else
            {
                item = CreateItem();
            }

            if (item != null)
            {
                item.SetActive(isActive);

                if (UGOPoolUtill.IsDebug)
                {
                    usedItemList.Add(new WeakReference<GameObject>(item));
                }
            }

            return item;
        }

        /// <summary>
        /// 从缓存池中得到指定类型的组件
        /// </summary>
        /// <typeparam name="T">继承于MonoBehaviour的组件</typeparam>
        /// <param name="isActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public T GetComponentItem<T>(bool isActive = true, bool addIfNot = false) where T : MonoBehaviour
        {
            if (templateGameObject.GetComponent<T>() == null && !addIfNot)
            {
                return null;
            }

            GameObject gObj = GetItem(isActive);
            if (gObj != null)
            {
                T component = gObj.GetComponent<T>();
                if (component == null && addIfNot)
                {
                    component = gObj.AddComponent<T>();
                }
                if(component == null)
                {
                    ReleaseItem(gObj);
                }
                return component;
            }

            return null;
        }

        private GameObject CreateItem()
        {
            GameObject item = null;
            if (templateType == UGOTemplateType.RuntimeInstance)
            {
                item = UnityObject.Instantiate(templateGameObject);
            }
            else
            {
                item = (GameObject)UGOPoolUtill.InstantiateProvider(AssetPath, templateGameObject);
            }
            return item;
        }

        private void DestroyItem(GameObject item)
        {
            if(templateType == UGOTemplateType.RuntimeInstance)
            {
                UnityObject.Destroy(item);
            }
            else
            {
                UGOPoolUtill.DestroyProvider(AssetPath, item);
            }
        }
        #endregion

        #region Release Item
        /// <summary>
        /// 回收GameObject
        /// </summary>
        /// <param name="item"></param>
        public void ReleaseItem(GameObject item)
        {
            if (item == null)
            {
                LogUtil.Error(UGOPoolUtill.LOG_TAG,"GameObjectPool::ReleaseItem->Item is Null");
                return;
            }

            if (UGOPoolUtill.IsDebug)
            {
                //从使用列表中删除要回收的对象
                for (int i = usedItemList.Count - 1; i >= 0; i--)
                {
                    if (usedItemList[i].TryGetTarget(out GameObject target) && !target.IsNull())
                    {
                        if (target != item)
                        {
                            continue;
                        }
                        else
                        {
                            usedItemList.RemoveAt(i);
                            break;
                        }
                    }
                    else
                    {
                        usedItemList.RemoveAt(i);
                    }
                }
            }

            if (unusedItemQueue.Count >= cachedMaxAmount)
            {
                DestroyItem(item);
                return;
            }

            item.transform.SetParent(poolTransform, false);
            item.SetActive(false);
            unusedItemQueue.Enqueue(item);
        }
        #endregion

        /// <summary>
        /// 销毁缓存池
        /// </summary>
        internal void Destroy()
        {
            preloadCompleteCallback = null;
            usedItemList.Clear();

            while(unusedItemQueue.Count>0)
            {
                DestroyItem(unusedItemQueue.Dequeue());
            }

            if(templateType == UGOTemplateType.RuntimeInstance)
            {
                UnityObject.Destroy(templateGameObject);
            }
            else if(templateType == UGOTemplateType.PrefabInstance)
            {
                UGOPoolUtill.DestroyProvider(AssetPath, templateGameObject);
            }
            templateGameObject = null;
            AssetPath = null;

            if (UGOPoolUtill.IsDebug)
            {
                UnityObject.Destroy(poolTransform.gameObject);
            }
        }
    }
}
