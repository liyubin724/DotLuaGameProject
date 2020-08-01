using DotEngine.Log;
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
        /// 缓存池所属的分组
        /// </summary>
        private GameObjectPoolGroup spawnPool = null;

        /// <summary>
        ///唯一名称，一般情况下为资源的路径
        /// </summary>
        private string uniqueName = null;
        private PoolTemplateType templateType = PoolTemplateType.Prefab;
        /// <summary>
        /// 缓存池中使用的GameObject的模板
        /// </summary>
        private GameObject instanceOrPrefabTemplate = null;
        internal GameObject Template { get => instanceOrPrefabTemplate; }

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
        /// <summary>
        /// 缓存池预加载完后回调
        /// </summary>
        public PoolPreloadComplete PreloadCompleteCallback = null;

        //预加载的GameObject数量
        public int PreloadTotalAmount { get; set; } = 0;
        //每帧加载的数量，可以防止一次大量的预加载造成卡顿，一般可以在Loading中提前处理
        public int PreloadOnceAmount { get; set; } = 1;

        //是否自动定时的清理缓存池中空闲的GameObject
        public bool IsAutoCull { get; set; } = false;
        //进行清理时，一次性清理的数量，为0表示一次性清理所有的
        public int CullOnceAmount { get; set; } = 0;
        //两次清理执行时间间隔,以秒为单位
        public int CullDelayTime { get; set; } = 30;

        //缓存池中GameObejct对象可以创建的最大的上限，如果超出则无法生成新的GameObject
        //值为0时，表示无限制
        public int LimitMaxAmount = 0;
        //缓存池清理时，池中至少保持的GameObject的数量下限
        //值为0时表示无限制
        public int LimitMinAmount = 0;

        //预加载的定时器
        private TimerHandler preloadTimerTask = null;

        internal GameObjectPool(GameObjectPoolGroup pool, string aPath, GameObject templateGObj, PoolTemplateType templateType)
        {
            spawnPool = pool;
            uniqueName = aPath;

            instanceOrPrefabTemplate = templateGObj;
            this.templateType = templateType;

            if(templateType != PoolTemplateType.Prefab)
            {
                instanceOrPrefabTemplate.SetActive(false);
                instanceOrPrefabTemplate.transform.SetParent(pool.GroupTransform, false);
            }
            TimerService timerService = Facade.GetInstance().GetService<TimerService>(TimerService.NAME);
            preloadTimerTask = timerService.AddIntervalTimer(0.05f, OnPreloadTimerUpdate);
        }

        #region Preload
        /// <summary>
        /// 使用Timer的Tick进行预加载
        /// </summary>
        /// <param name="obj"></param>
        private void OnPreloadTimerUpdate(SystemObject obj)
        {
            int curAmount = unusedItemQueue.Count;
            if (curAmount >= PreloadTotalAmount)
            {
                OnPoolPreloadComplete();
            }
            else
            {
                int poa = PreloadOnceAmount;
                if (poa == 0)
                {
                    poa = PreloadTotalAmount;
                }
                else
                {
                    poa = Mathf.Min(PreloadOnceAmount, PreloadTotalAmount - curAmount);
                }
                for (int i = 0; i < poa; ++i)
                {
                    GameObject instance = CreateNewItem();
                    instance.transform.SetParent(spawnPool.GroupTransform, false);
                    instance.SetActive(false);
                    unusedItemQueue.Enqueue(instance);
                }
            }
        }

        private void OnPoolPreloadComplete()
        {
            if (preloadTimerTask != null)
            {
                TimerService timerService = Facade.GetInstance().GetService<TimerService>(TimerService.NAME);
                timerService.RemoveTimer(preloadTimerTask);
                preloadTimerTask = null;
            }

            PreloadCompleteCallback?.Invoke(spawnPool.GroupName, uniqueName);
            PreloadCompleteCallback = null;
        }

        #endregion

        #region GetItem
        /// <summary>
        /// 从缓存池中得到一个GameObject对象
        /// </summary>
        /// <param name="isAutoSetActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public GameObject GetPoolItem(bool isAutoSetActive = true)
        {
            if (LimitMaxAmount != 0 && GetUsedItemCount() > LimitMaxAmount)
            {
                LogUtil.LogWarning(GameObjectPoolConst.LOGGER_NAME, "GameObjectPool::GetItem->Large than Max Amount");
                return null;
            }

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

                if (isAutoSetActive)
                {
                    item.SetActive(true);
                }
                usedItemList.Add(new WeakReference<GameObject>(item));
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
            if(instanceOrPrefabTemplate.GetComponent<T> ()==null && !autoAddIfNot)
            {
                return null;
            }

            GameObject gObj = GetPoolItem(isAutoActive);
            T component = null;
            if(gObj!=null)
            {
                component = gObj.GetComponent<T>();
                if(component == null)
                {
                    component = gObj.AddComponent<T>();

                    if(component is GameObjectPoolItem poolItem)
                    {
                        poolItem.SpawnName = spawnPool.GroupName;
                        poolItem.AssetPath = uniqueName;
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
                item = GameObject.Instantiate(instanceOrPrefabTemplate);
            }
            else
            {
                item = (GameObject)GameObjectPoolConst.InstantiateAsset(uniqueName, instanceOrPrefabTemplate);
            }

            if (item != null)
            {
                GameObjectPoolItem poolItem = item.GetComponent<GameObjectPoolItem>();
                if (poolItem != null)
                {
                    poolItem.AssetPath = uniqueName;
                    poolItem.SpawnName = spawnPool.GroupName;
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
        public void ReleasePoolItem(GameObject item)
        {
            if(item == null)
            {
                LogUtil.LogError(GameObjectPoolConst.LOGGER_NAME, "GameObjectPool::ReleaseItem->Item is Null");
                return;
            }

            GameObjectPoolItem pItem = item.GetComponent<GameObjectPoolItem>();
            if(pItem!=null)
            {
                pItem.DoDespawned();
            }

            item.transform.SetParent(spawnPool.GroupTransform, false);
            item.SetActive(false);
            unusedItemQueue.Enqueue(item);

            //从使用列表中删除要回收的对象
            for (int i = usedItemList.Count - 1; i >= 0; i--)
            {
                if(usedItemList[i].TryGetTarget(out GameObject target))
                {
                    if(!target.IsNull())
                    {
                        if(target!=item)
                        {
                            continue;
                        }else
                        {
                            usedItemList.RemoveAt(i);
                            break;
                        }
                    }
                }else
                {
                    usedItemList.RemoveAt(i);
                }
            }
        }
        #endregion

        private float cullTime = 0;
        internal void CullPool(float deltaTime)
        {
            CleanUsedItem();
            if (!IsAutoCull)
            {
                return;
            }

            cullTime += deltaTime;
            if(cullTime < CullDelayTime)
            {
                return;
            }
            cullTime = 0;

            //计算一次裁剪的数量
            int cullAmout = 0;
            if (usedItemList.Count + unusedItemQueue.Count <= LimitMinAmount)
            {
                cullAmout = 0;
            }
            else
            {
                cullAmout = usedItemList.Count + unusedItemQueue.Count - LimitMinAmount;
                if (cullAmout > unusedItemQueue.Count)
                {
                    cullAmout = unusedItemQueue.Count;
                }
            }

            if (CullOnceAmount > 0 && CullOnceAmount < cullAmout)
            {
                cullAmout = CullOnceAmount;
            }

            for (int i = 0; i < cullAmout && unusedItemQueue.Count>0; i++)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
            }
        }
        
        /// <summary>
        /// 销毁缓存池
        /// </summary>
        internal void DestroyPool()
        {
            PreloadCompleteCallback = null;
            if (preloadTimerTask != null)
            {
                TimerService timerService = Facade.GetInstance().GetService<TimerService>(TimerService.NAME);
                timerService.RemoveTimer(preloadTimerTask);
                preloadTimerTask = null;
            }

            usedItemList.Clear();

            for (int i = unusedItemQueue.Count - 1; i >= 0; i--)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
            }
            unusedItemQueue.Clear();

            if(templateType == PoolTemplateType.PrefabInstance || templateType == PoolTemplateType.RuntimeInstance)
            {
                UnityObject.Destroy(instanceOrPrefabTemplate);
            }
            instanceOrPrefabTemplate = null;

            uniqueName = null;
            spawnPool = null;
        }

        private int GetUsedItemCount()
        {
            CleanUsedItem();
            return usedItemList.Count;
        }

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
    }
}
