using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public class AsyncResult
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

        public void DoInitilize(int id,string[] addresses)
        {
            Id = id;
            Addresses = new string[addresses.Length];
            Progresses = new float[addresses.Length];
            UObjects = new UnityObject[addresses.Length];
            m_Flags = new bool[addresses.Length];
            for(int i =0;i<addresses.Length;i++)
            {
                Addresses[i] = addresses[i];
                Progresses[i] = 0.0f;
                UObjects[i] = null;
                m_Flags[i] = false;
            }
        }

        public void DoDestroy()
        {
            Id = 0;
            Addresses = null;
            Progresses = null;
            UObjects = null;
        }
    }
}
