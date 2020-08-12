using System;

namespace DotEngine.Sampler
{
    public enum RecordCategory
    {
        None = 0,
        FPS,
        Log,
        Memory,
        System,
    }

    public class RecordData 
    {
        public RecordCategory category;
        public DateTime dateTime;
        
        protected RecordData(RecordCategory c)
        {
            category = c;
            dateTime = DateTime.Now;
        }
    }
}
