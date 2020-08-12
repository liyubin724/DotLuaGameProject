using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Sampler
{
    public abstract class ASampler
    {
        public int SamplingFrame { get; set; } = 5;
        public float SynInterval { get; set; } = 1.0f;

        private int currentFrame = 0;
        private float elapsedTime = 0.0f;
        private List<RecordData> records = new List<RecordData>();
        public void DoUpdate(float deltaTime)
        {
            if(SamplingFrame<=0)
            {
                return;
            }

            currentFrame++;
            if(currentFrame % SamplingFrame == 0)
            {
                records.Add(Sample());
                currentFrame = 0;
            }
            elapsedTime += deltaTime;
            if (elapsedTime >= SynInterval)
            {

            }

        }

        protected abstract RecordData Sample();

        public RecordData[] GetRecords()
        {
            var result = records.ToArray();

            records.Clear();
            return result;
        }
    }
}
