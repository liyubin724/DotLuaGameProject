using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.BD.Executors
{
    public class BDExecutorFactory
    {
        private static BDExecutorFactory sm_Factory = null;
        public static BDExecutorFactory GetInstance()
        {
            if(sm_Factory == null)
            {
                sm_Factory = new BDExecutorFactory();
                sm_Factory.RegistPoolByReflection();
            }
            return sm_Factory;
        }

        private Dictionary<Type, ObjectPool> m_PoolDic = new Dictionary<Type, ObjectPool>();

        private BDExecutorFactory()
        {

        }

        public void RegistPool(Type dataType,ObjectPool pool)
        {
            if(!m_PoolDic.ContainsKey(dataType))
            {
                m_PoolDic.Add(dataType, pool);
            }
        }

        public void UnregistPool(Type dataType)
        {
            if(m_PoolDic.ContainsKey(dataType))
            {
                m_PoolDic.Remove(dataType);
            }
        }

        public void ClearPool()
        {
            foreach(var kvp in m_PoolDic)
            {
                kvp.Value.Clear();
            }
            m_PoolDic.Clear();
        }

        public T RetainExecutor<T>(Type dataType) where T: BDExecutor
        {
            if (m_PoolDic.TryGetValue(dataType, out var pool))
            {
                return (T)pool.Get();
            }
            return null;
        }

        public void ReleaseExecutor(BDExecutor executor)
        {
            if (m_PoolDic.TryGetValue(executor.Data.GetType(), out var pool))
            {
                pool.Release(executor);
            }
        }

        private void RegistPoolByReflection()
        {
            Type[] types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && type.IsSubclassOf(typeof(BDExecutor))
                            select type).ToArray();

            for(int i =0;i<types.Length;++i)
            {
                var attr = types[i].GetCustomAttribute(typeof(BDExecutorAttribute));
                if(attr!=null)
                {
                    BDExecutorAttribute aeAttr = (BDExecutorAttribute)attr;
                    ObjectPool pool = new ObjectPool(() =>
                    {
                        return Activator.CreateInstance(types[i]);
                    }, null, (executor) =>
                    {
                        ((BDExecutor)executor).DoReset();
                    }, 0);

                    RegistPool(aeAttr.BindDataType, pool);
                }
            }
        }

    }
}
