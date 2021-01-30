using DotEngine.Pool;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.BehaviourLine.Action
{
    public class ActionItemFactory
    {
        private Dictionary<Type, ObjectPool> itemPoolDic = new Dictionary<Type, ObjectPool>();

        private static ActionItemFactory itemFactory = null;

        private ActionItemFactory() { }

        public static ActionItemFactory GetInstance()
        {
            if (itemFactory == null)
            {
                itemFactory = new ActionItemFactory();
            }
            return itemFactory;
        }

        public void RegisterItemPool(Type dataType, ObjectPool itemPool)
        {
            if (!itemPoolDic.ContainsKey(dataType))
            {
                itemPoolDic.Add(dataType, itemPool);
            }
        }

        public ActionItem RetainItem(Type dataType)
        {
            if (itemPoolDic.TryGetValue(dataType, out ObjectPool itemPool))
            {
                return (ActionItem)itemPool.Get();
            }
            return null;
        }

        public void ReleaseItem(ActionItem item)
        {
            Type dataType = item.Data.GetType();
            if (itemPoolDic.TryGetValue(dataType, out ObjectPool itemPool))
            {
                itemPool.Release(item);
            }
        }

        public void DoClear()
        {
            foreach (var kvp in itemPoolDic)
            {
                kvp.Value.Clear();
            }
            itemPoolDic.Clear();
        }

        public void DoDestroy()
        {
            DoClear();

            itemFactory = null;
        }

        public void RegisterItemPoolByReflection()
        {
            Type[] itemTypes = AssemblyUtility.GetDerivedTypes(typeof(ActionItem));
            if (itemTypes == null || itemTypes.Length == 0)
            {
                return;
            }

            Dictionary<Type, Type> dataToItemTypeDic = new Dictionary<Type, Type>();
            foreach (var itemType in itemTypes)
            {
                ActionItemBindDataAttribute attr = itemType.GetCustomAttribute<ActionItemBindDataAttribute>();
                if (attr == null)
                {
                    continue;
                }

                dataToItemTypeDic.Add(attr.DataType, itemType);
            }

            foreach (var kvp in dataToItemTypeDic)
            {
                ObjectPool itemPool = new ObjectPool(() =>
                {
                    return Activator.CreateInstance(kvp.Value);
                }, null, (actionItem) =>
                {
                    ((ActionItem)actionItem).DoReset();
                }, 0);

                RegisterItemPool(kvp.Key, itemPool);
            }
        }
    }
}
