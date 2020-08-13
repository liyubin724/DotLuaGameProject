using DotEngine.Pool;
using System;

namespace DotEngine.PMonitor.Sampler
{
    public class Record : IObjectPoolItem
    {
        public SamplerCategory Category { get; set; }
        public DateTime Time { get; set; }

        public Record()
        {

        }

        public virtual void OnNew()
        {
        }

        public void OnGet()
        {
            Time = DateTime.Now;
        }

        public void OnRelease()
        {
        }
    }
}
