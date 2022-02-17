﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public delegate void PreloadComplete(string groupName, string assetPath);

    /// <summary>
    /// 用于创建缓存池的模板的类型
    /// </summary>
    public enum TemplateType
    {
        Prefab = 0,//使用Prefab做为缓存池模型
        PrefabInstance,//使用Prefab实例化后的对象做为模板
        RuntimeInstance,//运行时创建的对象做模板
    }

    /// <summary>
    /// 以GameObject为对象的缓存池
    /// </summary>
    public class PoolGroup
    {
        public string categoryName;
        private Transform groupTransform;

        /// <summary>
        /// 资源标识，一般使用资源的路径或者指定一个唯一的名称
        /// </summary>
        public string assetPath;
        public TemplateType templateType;
        /// <summary>
        /// 缓存池中使用的GameObject的模板
        /// </summary>
        private GameObject template = null;

        /// <summary>
        /// 缓存池预加载完后回调
        /// </summary>
        private PreloadComplete preloadCompleteCallback = null;
        //预加载的GameObject数量
        private int preloadTotalAmount = 0;
        //每帧加载的数量，可以防止一次大量的预加载造成卡顿，一般可以在Loading中提前处理
        private int preloadOnceAmount = 1;

        private bool isCullEnable = true;
        //进行清理时，一次性清理的数量，为0表示一次性清理所有的
        private int cullOnceAmount  = 0;
        //两次清理执行时间间隔,以秒为单位
        private float cullDelayTime = 60.0f;
        private float preCullTime = 0.0f;

        //缓存池中GameObejct对象可以创建的最大的上限，如果超出则无法生成新的GameObject
        //值为0时，表示无限制
        private int limitMaxAmount = int.MaxValue;
        //缓存池清理时，池中至少保持的GameObject的数量下限
        //值为0时表示无限制
        private int limitMinAmount = 0;

        /// <summary>
        /// 空闲的GameObject对象栈
        /// </summary>
        private Queue<GameObject> unusedItemQueue = new Queue<GameObject>();

        /// <summary>
        /// 从缓存池中获取的正在使用的对象
        /// 为了上层方便对GameObject管理，采用弱引用的方式存储，
        /// 可以保证即使是上层删除了GameObject也不会对整体造成影响
        /// </summary>
        private List<WeakReference<GameObject>> usedItemList = new List<WeakReference<GameObject>>();

        internal PoolGroup(
            string categoryName,
            Transform parentTransform, 
            string assetPath, 
            TemplateType templateType, 
            GameObject templateGObj)
        {
            this.categoryName = categoryName;
            if(PoolUtill.IsDebug)
            {
                GameObject gObj = new GameObject(assetPath);
                groupTransform = gObj.transform;

                groupTransform.SetParent(parentTransform, false);
            }else
            {
                groupTransform = parentTransform;
            }

            this.assetPath = assetPath;
            this.templateType = templateType;
            template = templateGObj;

            if(templateType != TemplateType.Prefab)
            {
                template.SetActive(false);
                template.transform.SetParent(groupTransform, false);
            }
        }

        internal void DoUpdate(float deltaTime)
        {
            //preload
            if(preloadTotalAmount > 0)
            {
                PreloadItem();
            }else if(preloadCompleteCallback!=null)
            {
                preloadCompleteCallback.Invoke(categoryName, assetPath);
                preloadCompleteCallback = null;
            }

            //cull
            if(isCullEnable && cullDelayTime > 0)
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
        public void SetPreload(int totalAmount, int onceAmount, PreloadComplete callback = null)
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
                instance.transform.SetParent(groupTransform, false);
                instance.SetActive(false);

                unusedItemQueue.Enqueue(instance);
            }

            preloadTotalAmount -= amount;
        }
#endregion

#region Cull
        public void SetCullEnable(bool enable)
        {
            isCullEnable = enable;
        }

        public void SetCull(int onceAmount, float delayTime)
        {
            cullOnceAmount = onceAmount;
            cullDelayTime = delayTime;
        }

        private void Cull()
        {
            if(PoolUtill.IsDebug)
            {
                for (int i = usedItemList.Count - 1; i >= 0; --i)
                {
                    if (usedItemList[i].TryGetTarget(out GameObject gObj) && gObj.IsNull())
                    {
                        usedItemList.RemoveAt(i);
                    }
                }
            }

            if (unusedItemQueue.Count <= limitMinAmount)
            {
                return;
            }
            int amount = unusedItemQueue.Count - limitMinAmount;
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

#region Limit
        public void SetLimit(int minAmount,int maxAmount)
        {
            limitMinAmount = minAmount;
            limitMaxAmount = maxAmount;
        }
#endregion

#region GetItem
        /// <summary>
        /// 从缓存池中得到一个GameObject对象
        /// </summary>
        /// <param name="isAutoSetActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public GameObject GetItem(bool isAutoSetActive = true)
        {
            GameObject item = null;
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
                item.SetActive(isAutoSetActive);
                //item.BroadcastMessage("DoGet", SendMessageOptions.DontRequireReceiver);

                if(PoolUtill.IsDebug)
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
        /// <param name="isAutoActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public T GetComponentItem<T>(bool isAutoActive = true,bool autoAddIfNot = false) where T:MonoBehaviour
        {
            if(template.GetComponent<T> ()==null && !autoAddIfNot)
            {
                return null;
            }

            GameObject gObj = GetItem(isAutoActive);
            T component = null;
            if(gObj!=null)
            {
                component = gObj.GetComponent<T>();
                if(component == null && autoAddIfNot)
                {
                    component = gObj.AddComponent<T>();
                }
            }

            return component;
        }

        private GameObject CreateItem()
        {
            GameObject item = null;
            if(templateType == TemplateType.RuntimeInstance)
            {
                item = UnityObject.Instantiate(template);
            }
            else
            {
                item = (GameObject)PoolUtill.InstantiateProvider(assetPath, template);
            }
            return item;
        }

        private void DestroyItem(GameObject item)
        {
            PoolUtill.DestroyProvider(assetPath, item);
        }
#endregion

#region Release Item
        /// <summary>
        /// 回收GameObject
        /// </summary>
        /// <param name="item"></param>
        public void ReleaseItem(GameObject item)
        {
            if(item == null)
            {
                PoolUtill.Error("GameObjectPool::ReleaseItem->Item is Null");
                return;
            }

            if(unusedItemQueue.Count > limitMaxAmount)
            {
                DestroyItem(item);
                return;
            }

            item.transform.SetParent(groupTransform, false);
            item.SetActive(false);
            unusedItemQueue.Enqueue(item);

            if(PoolUtill.IsDebug)
            {
                //从使用列表中删除要回收的对象
                for (int i = usedItemList.Count - 1; i >= 0; i--)
                {
                    if(usedItemList[i].TryGetTarget(out GameObject target) && !target.IsNull())
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
        }
#endregion

        /// <summary>
        /// 销毁缓存池
        /// </summary>
        internal void DoDestroy()
        {
            preloadCompleteCallback = null;
            usedItemList.Clear();

            for (int i = unusedItemQueue.Count - 1; i >= 0; i--)
            {
                DestroyItem(unusedItemQueue.Dequeue());
            }
            unusedItemQueue.Clear();

            if(templateType == TemplateType.PrefabInstance || templateType == TemplateType.RuntimeInstance)
            {
                DestroyItem(template);
            }
            template = null;

            assetPath = null;
            categoryName = null;
            if(PoolUtill.IsDebug)
            {
                UnityObject.Destroy(groupTransform.gameObject);
            }
        }

    }
}
