using DotEngine.Timer;
using DotEngine.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOPool
{
    public delegate void PoolPreloadComplete(string groupName, string assetPath);

    /// <summary>
    /// 用于创建缓存池的模板的类型
    /// </summary>
    public enum PoolTemplateType
    {
        Prefab,//使用Prefab做为缓存池模型
        PrefabInstance,//使用Prefab实例化后的对象做为模板
        RuntimeInstance,//运行时创建的对象做模板
    }

    /// <summary>
    /// 以GameObject为对象的缓存池
    /// </summary>
    public class GameObjectPool
    {
        /// <summary>
        /// 空闲的GameObject对象栈
        /// </summary>
        private Queue<GameObject> unusedItemQueue = new Queue<GameObject>();

#if UNITY_EDITOR
        /// <summary>
        /// 从缓存池中获取的正在使用的对象
        /// 为了上层方便对GameObject管理，采用弱引用的方式存储，
        /// 可以保证即使是上层删除了GameObject也不会对整体造成影响
        /// </summary>
        private List<WeakReference<GameObject>> usedItemList = new List<WeakReference<GameObject>>();
#endif

        private string groupName;
        private Transform parentTransform;
        /// <summary>
        ///唯一名称，一般情况下为资源的路径
        /// </summary>
        private string poolName = null;
        private PoolTemplateType templateType = PoolTemplateType.Prefab;
        /// <summary>
        /// 缓存池中使用的GameObject的模板
        /// </summary>
        private GameObject templateGameObject = null;

        /// <summary>
        /// 缓存池预加载完后回调
        /// </summary>
        private PoolPreloadComplete preloadCompleteCallback = null;
        //预加载的GameObject数量
        private int preloadTotalAmount = 0;
        //每帧加载的数量，可以防止一次大量的预加载造成卡顿，一般可以在Loading中提前处理
        private int preloadOnceAmount = 1;
        //预加载的定时器
        private TimerHandler preloadTimerHandler = null;

        //进行清理时，一次性清理的数量，为0表示一次性清理所有的
        private int cullOnceAmount  = 0;
        //两次清理执行时间间隔,以秒为单位
        private float cullDelayTime = 60.0f;
        private TimerHandler cullTimerHandler = null;

        //缓存池中GameObejct对象可以创建的最大的上限，如果超出则无法生成新的GameObject
        //值为0时，表示无限制
        private int limitMaxAmount = int.MaxValue;
        //缓存池清理时，池中至少保持的GameObject的数量下限
        //值为0时表示无限制
        private int limitMinAmount = 0;

        internal GameObjectPool(string groupName,Transform parentTransform, string poolName, PoolTemplateType templateType, GameObject templateGObj)
        {
            this.groupName = groupName;
            this.poolName = poolName;
            this.parentTransform = parentTransform;
            this.templateType = templateType;
            templateGameObject = templateGObj;

            if(templateType != PoolTemplateType.Prefab)
            {
                templateGameObject.SetActive(false);
                templateGameObject.transform.SetParent(this.parentTransform, false);
            }
        }

        #region Preload
        public void SetPreload(int totalAmount, int onceAmount, PoolPreloadComplete callback = null)
        {
            preloadTotalAmount = totalAmount;
            preloadOnceAmount = onceAmount;

            preloadCompleteCallback = callback;

            TimerService timerService = Facade.GetInstance().GetServicer<TimerService>(TimerService.NAME);
            if (preloadTimerHandler != null)
            {
                timerService.RemoveTimer(preloadTimerHandler);
                preloadTimerHandler = null;
            }

            if (preloadTotalAmount <= 0)
            {
                OnPreloadComplete();
            }
            else
            {
                preloadTimerHandler = timerService.AddTickTimer(OnPreloadTimerUpdate, null);
            }
        }

        /// <summary>
        /// 使用Timer的Tick进行预加载
        /// </summary>
        /// <param name="obj"></param>
        private void OnPreloadTimerUpdate(SystemObject obj)
        {
            if(preloadTotalAmount > 0)
            {
                int amount = preloadTotalAmount;
                if (preloadOnceAmount > 0)
                {
                    amount = Mathf.Min(preloadOnceAmount, amount);
                }
                for (int i = 0; i < amount; ++i)
                {
                    GameObject instance = CreateNewItem();
                    instance.transform.SetParent(parentTransform, false);
                    instance.SetActive(false);

                    unusedItemQueue.Enqueue(instance);
                }

                preloadTotalAmount -= amount;
            }
            else
            {
                OnPreloadComplete();
            }
        }

        private void OnPreloadComplete()
        {
            if (preloadTimerHandler != null)
            {
                TimerService timerService = Facade.GetInstance().GetServicer<TimerService>(TimerService.NAME);
                timerService.RemoveTimer(preloadTimerHandler);
                preloadTimerHandler = null;
            }

            preloadTotalAmount = preloadOnceAmount = 0;

            preloadCompleteCallback?.Invoke(groupName, poolName);
            preloadCompleteCallback = null;
        }

        #endregion

        #region Cull
        public void SetCull(int onceAmount,float delayTime)
        {
            cullOnceAmount = onceAmount;
            cullDelayTime = delayTime;

            TimerService timerService = Facade.GetInstance().GetServicer<TimerService>(TimerService.NAME);
            if (cullTimerHandler != null)
            {
                timerService.RemoveTimer(cullTimerHandler);
                cullTimerHandler = null;
            }
            if(cullOnceAmount>0)
            {
                cullTimerHandler = timerService.AddIntervalTimer(cullDelayTime, OnCullTimerUpdate, null);
            }
        }

        private void OnCullTimerUpdate(SystemObject userdata)
        {
#if UNITY_EDITOR
            CleanUsedItem();
#endif

            if (unusedItemQueue.Count <= limitMinAmount)
            {
                return;
            }
            int amount = unusedItemQueue.Count - limitMinAmount;
            if(cullOnceAmount > 0)
            {
                amount = Mathf.Min(amount, cullOnceAmount);
            }
            for(int i =0;i<amount;++i)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
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
                item = CreateNewItem();
            }

            if (item != null)
            {
                GameObjectPoolItem poolItem = item.GetComponent<GameObjectPoolItem>();
                if (poolItem != null)
                {
                    poolItem.DoSpawned();
                }

                item.SetActive(isAutoSetActive);
#if UNITY_EDITOR
                usedItemList.Add(new WeakReference<GameObject>(item));
#endif
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
            if(templateGameObject.GetComponent<T> ()==null && !autoAddIfNot)
            {
                return null;
            }

            GameObject gObj = GetItem(isAutoActive);
            T component = null;
            if(gObj!=null)
            {
                component = gObj.GetComponent<T>();
                if(component == null)
                {
                    component = gObj.AddComponent<T>();

                    if(component is GameObjectPoolItem poolItem)
                    {
                        poolItem.GroupName = groupName;
                        poolItem.PoolName = poolName;

                        poolItem.DoSpawned();
                    }
                }
            }

            return component;
        }

        private GameObject CreateNewItem()
        {
            GameObject item = null;
            if(templateType == PoolTemplateType.RuntimeInstance)
            {
                item = GameObject.Instantiate(templateGameObject);
            }
            else
            {
                item = (GameObject)GameObjectPoolConst.InstantiateAsset(poolName, templateGameObject);
            }

            if (item != null)
            {
                GameObjectPoolItem poolItem = item.GetComponent<GameObjectPoolItem>();
                if (poolItem != null)
                {
                    poolItem.GroupName = groupName;
                    poolItem.PoolName = poolName;
                }
            }
            return item;
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
                DebugLog.Error(GameObjectPoolConst.LOGGER_NAME, "GameObjectPool::ReleaseItem->Item is Null");
                return;
            }

            GameObjectPoolItem pItem = item.GetComponent<GameObjectPoolItem>();
            if(pItem!=null)
            {
                pItem.DoDespawned();
            }

            if(unusedItemQueue.Count > limitMaxAmount)
            {
                UnityObject.Destroy(item);
                return;
            }

            item.transform.SetParent(parentTransform, false);
            item.SetActive(false);
            unusedItemQueue.Enqueue(item);

#if UNITY_EDITOR
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
#endif
        }
        #endregion

        /// <summary>
        /// 销毁缓存池
        /// </summary>
        internal void Dispose()
        {
            preloadCompleteCallback = null;
            TimerService timerService = Facade.GetInstance().GetServicer<TimerService>(TimerService.NAME);
            if (preloadTimerHandler != null)
            {
                timerService.RemoveTimer(preloadTimerHandler);
                preloadTimerHandler = null;
            }

            if (cullTimerHandler != null)
            {
                timerService.RemoveTimer(cullTimerHandler);
                cullTimerHandler = null;
            }
#if UNITY_EDITOR
            usedItemList.Clear();
#endif
            for (int i = unusedItemQueue.Count - 1; i >= 0; i--)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
            }
            unusedItemQueue.Clear();

            if(templateType == PoolTemplateType.PrefabInstance || templateType == PoolTemplateType.RuntimeInstance)
            {
                UnityObject.Destroy(templateGameObject);
            }
            templateGameObject = null;

            parentTransform = null;
            poolName = null;
            groupName = null;
        }
#if UNITY_EDITOR
        private void CleanUsedItem()
        {
            for(int i = usedItemList.Count -1;i>=0;--i)
            {
                if(usedItemList[i].TryGetTarget(out GameObject gObj) && gObj.IsNull())
                {
                    usedItemList.RemoveAt(i);
                }
            }
        }
#endif
    }
}
