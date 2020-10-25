using SystemObject = System.Object;

namespace KSTCEngine.GPerf
{
    class GPerfMonitorAction
    {
        public const string START_ACTION = "Start";
        public const string END_ACTION = "End";
        public const string OPEN_SAMPLER_ACTION = "OpenSampler";
        public const string CLOSE_SAMPLER_ACTION = "CloseSampler";
        public const string OPEN_RECORDER_ACTION = "OpenRecorder";
        public const string CLOSE_RECORDER_ACTION = "CloseRecorder";

        public string ActionName { get; set; }
        public SystemObject ParamValue { get; set; } = null;
    }
}
