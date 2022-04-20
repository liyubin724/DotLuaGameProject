using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public class AssetAsyncResult
    {
        public int Id { get; private set; }

        public string[] Addresses { get; private set; }
        public float[] Progresses { get; private set; }
        public UnityObject[] UObjects { get; private set; }

        private bool[] m_Flags = null;
        public bool IsDone
        {
            get
            {
                if(m_Flags == null)
                {
                    return false;
                }
                foreach(var flag in m_Flags)
                {
                    if(!flag)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public AssetAsyncResult(int id, string[] addresses)
        {
            Id = id;
            Addresses = new string[addresses.Length];
            Progresses = new float[addresses.Length];
            UObjects = new UnityObject[addresses.Length];
            m_Flags = new bool[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                Addresses[i] = addresses[i];
                Progresses[i] = 0.0f;
                UObjects[i] = null;
                m_Flags[i] = false;
            }
        }

        internal void SetProgress(int index,float progress)
        {
            Progresses[index] = progress;
        }

        internal void SetUObject(int index,UnityObject uObject)
        {
            UObjects[index] = uObject;
            m_Flags[index] = true;
        }
    }
}
