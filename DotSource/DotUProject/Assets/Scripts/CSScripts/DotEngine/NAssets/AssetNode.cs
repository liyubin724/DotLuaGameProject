using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotEngine.Pool;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public class AssetNode
    {
        public string Path { get; private set; }

        private bool m_IsLoaded = false;

        private WeakReference<UnityObject> assetWR = null;
        private List<WeakReference<UnityObject>> instanceWRList = null;

        private UnityObject m_Asset;
        private int m_RefCount = 0;
        internal void RetainRef()
        {
            assetWR = new WeakReference<UnityObject>(null);
            m_RefCount++;
        }

        internal void ReleaseRef()
        {
            m_RefCount--;
        }

        internal UnityObject GetAsset()
        {
            if(m_Asset!=null)
            {
                return m_Asset;
            }
            if(assetWR.TryGetTarget(out m_Asset))
            {
                return m_Asset;
            }
            return null;
        }

        internal void SetAsset(UnityObject asset)
        {
            if(m_IsLoaded)
            {
                throw new Exception();
            }

            m_IsLoaded = true;
            assetWR = new WeakReference<UnityObject>(asset);
            instanceWRList = ListPool<WeakReference<UnityObject>>.Get();
        }

        internal UnityObject CreateInstance()
        {
            UnityObject asset = GetAsset();
            if(asset !=null)
            {
                UnityObject instance = UnityObject.Instantiate(asset);
                bool isAdded = false;
                for(int i =0;i<instanceWRList.Count;i++)
                {
                    var wr = instanceWRList[i];
                    if(!wr.TryGetTarget(out var target))
                    {
                        isAdded = true;
                        wr.SetTarget(instance);
                        break;
                    }
                }
                if(!isAdded)
                {
                    instanceWRList.Add(new WeakReference<UnityObject>(instance));
                }
                return instance;
            }

            return null;
        }

        internal void DestroyInstance(UnityObject instance)
        {

        }

        internal void DoActivate(string path)
        {
            Path = path;
        }

        internal void DoDeactivate()
        {
            Path = null;
            assetWR = null;
            if(instanceWRList!=null)
            {
                ListPool<WeakReference<UnityObject>>.Release(instanceWRList);
            }
            instanceWRList = null;
        }
    }
}
