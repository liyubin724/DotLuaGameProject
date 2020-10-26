using System;
using System.Threading;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class SystemMemoryRecord : Record
    {
        public long TotalMemInKB { get; set; } = 0;
        public long AvailableMemInKB { get; set; } = 0;
        public long ThresholdInKB { get; set; } = 0;
        public bool IsLowMem { get; set; } = false;
        public long PSSMemInKB { get; set; } = 0;
    }

    public class SystemMemorySampler : GPerfSampler<SystemMemoryRecord>
    {
        public const string MEMORY_TOTAL_KEY = "totalMem";
        public const string MEMORY_AVAILABLE_KEY = "availMem";
        public const string MEMORY_THRESHOLD_KEY = "threshold";
        public const string MEMORY_IS_LOW_KEY = "lowMemory";
        public const string MEMORY_PSS_KEY = "PSS";

        private long m_TotalMemInKB = 0L;
        private long m_AvailableMemInKB = 0L;
        private long m_ThresholdInKB = 0L;
        private bool m_IsLowMemInKB = false;
        private long m_PssMemInKB = 0L;

        public SystemMemorySampler()
        {
            MetricType = SamplerMetricType.SystemMemory;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        public long GetTotalMem()
        {
            return m_TotalMemInKB;
        }

        public long GetAvailableMem()
        {
            return m_AvailableMemInKB;
        }

        public long GetThreshold()
        {
            return m_ThresholdInKB;
        }

        public bool IsLowMem()
        {
            return m_IsLowMemInKB;
        }

        public long GetPssMem()
        {
            return m_PssMemInKB;
        }

        protected override void OnSample()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                AndroidJNI.AttachCurrentThread();
                {
                    string memoryInfo = GPerfPlatform.GetMemoryInfo();
                    if (!string.IsNullOrEmpty(memoryInfo))
                    {
                        string[] lines = memoryInfo.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines != null && lines.Length > 0)
                        {
                            foreach (var line in lines)
                            {
                                GPerfUtil.GetKeyValue(line, out var name, out var value);
                                if (name == MEMORY_TOTAL_KEY)
                                {
                                    if (!long.TryParse(value, out m_TotalMemInKB))
                                    {
                                        m_TotalMemInKB = 0L;
                                    }
                                    m_TotalMemInKB /= GPerfUtil.BYTE_TO_MB_SIZE;
                                }
                                else if (name == MEMORY_AVAILABLE_KEY)
                                {
                                    if (!long.TryParse(value, out m_AvailableMemInKB))
                                    {
                                        m_AvailableMemInKB = 0L;
                                    }
                                    m_AvailableMemInKB /= GPerfUtil.BYTE_TO_MB_SIZE;
                                }
                                else if (name == MEMORY_THRESHOLD_KEY)
                                {
                                    if (!long.TryParse(value, out m_ThresholdInKB))
                                    {
                                        m_ThresholdInKB = 0L;
                                    }
                                    m_ThresholdInKB /= GPerfUtil.BYTE_TO_MB_SIZE;
                                }
                                else if (name == MEMORY_IS_LOW_KEY)
                                {
                                    if (!bool.TryParse(value, out m_IsLowMemInKB))
                                    {
                                        m_IsLowMemInKB = false;
                                    }
                                }
                                else if (name == MEMORY_PSS_KEY)
                                {
                                    if (!long.TryParse(value, out m_PssMemInKB))
                                    {
                                        m_PssMemInKB = 0L;
                                    }
                                    m_PssMemInKB /= GPerfUtil.BYTE_TO_MB_SIZE;
                                }
                            }
                        }
                    }

                    record.TotalMemInKB = m_TotalMemInKB;
                    record.AvailableMemInKB = m_AvailableMemInKB;
                    record.IsLowMem = m_IsLowMemInKB;
                    record.ThresholdInKB = m_ThresholdInKB;
                    record.PSSMemInKB = m_PssMemInKB;
                }
                AndroidJNI.DetachCurrentThread();
            });
        }
    }
}
