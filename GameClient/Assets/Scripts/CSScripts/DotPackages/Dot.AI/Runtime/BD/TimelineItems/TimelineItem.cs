using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BD
{
    public abstract class TimelineItem
    {
        private float fireTime = 0.0f;
        public float FireTime
        {
            get
            {
                return FireTime;
            }
            set
            {
                fireTime = value;
                if(fireTime<0)
                {
                    fireTime = 0;
                }
            }
        }
    }
}
