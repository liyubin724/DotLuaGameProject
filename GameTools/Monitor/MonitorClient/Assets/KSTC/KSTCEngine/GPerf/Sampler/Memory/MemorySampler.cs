using System;

namespace KSTCEngine.GPerf.Sampler
{
    public class MemoryRecord : Record
    {
        public long TotalMem { get; set; } = 0L;
        public long AvailableMem { get; set; } = 0L;
        public long Threshold { get; set; } = 0L;
        public bool IsLowMem { get; set; } = false;
        public long PSSMem { get; set; } = 0L;
    }

    public class MemorySampler : GPerfSampler<MemoryRecord>
    {
        public const string MEMORY_TOTAL_KEY = "totalMem";
        public const string MEMORY_AVAILABLE_KEY = "availMem";
        public const string MEMORY_THRESHOLD_KEY = "threshold";
        public const string MEMORY_IS_LOW_KEY = "lowMemory";
        public const string MEMORY_PSS_KEY = "PSS";

        private long m_TotalMem = 0L;
        private long m_AvailableMem = 0L;
        private long m_Threshold = 0L;
        private bool m_IsLowMem = false;
        private long m_PssMem = 0L;

        public MemorySampler()
        {
            MetricType = SamplerMetricType.Memory;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 10.0f;
        }

        public long GetTotalMem()
        {
            return m_TotalMem;
        }

        public long GetAvailableMem()
        {
            return m_AvailableMem;
        }

        public long GetThreshold()
        {
            return m_Threshold;
        }

        public bool IsLowMem()
        {
            return m_IsLowMem;
        }

        public long GetPssMem()
        {
            return m_PssMem;
        }

        protected override void OnSample(MemoryRecord record)
        {
            string memoryInfo = GPerfPlatform.GetMemoryInfo();
            if(!string.IsNullOrEmpty(memoryInfo))
            {
                string[] lines = memoryInfo.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if(lines !=null && lines.Length>0)
                {
                    foreach(var line in lines)
                    {
                        GPerfUtil.GetKeyValue(line, out var name, out var value);
                        if (name == MEMORY_TOTAL_KEY)
                        {
                            if(!long.TryParse(value,out m_TotalMem))
                            {
                                m_TotalMem = 0L;
                            }
                        }
                        else if (name == MEMORY_AVAILABLE_KEY)
                        {
                            if (!long.TryParse(value, out m_AvailableMem))
                            {
                                m_AvailableMem = 0L;
                            }
                        }
                        else if (name == MEMORY_THRESHOLD_KEY)
                        {
                            if (!long.TryParse(value, out m_Threshold))
                            {
                                m_Threshold = 0L;
                            }
                        }
                        else if(name == MEMORY_IS_LOW_KEY)
                        {
                            if(!bool.TryParse(value,out m_IsLowMem))
                            {
                                m_IsLowMem = false;
                            }
                        }else if(name == MEMORY_PSS_KEY)
                        {
                            if(!long.TryParse(value,out m_PssMem))
                            {
                                m_PssMem = 0L;
                            }
                        }
                    }
                }
            }

            record.TotalMem = m_TotalMem;
            record.AvailableMem = m_TotalMem;
            record.IsLowMem = m_IsLowMem;
            record.Threshold = m_Threshold;
            record.PSSMem = m_PssMem;
        }
    }
}
