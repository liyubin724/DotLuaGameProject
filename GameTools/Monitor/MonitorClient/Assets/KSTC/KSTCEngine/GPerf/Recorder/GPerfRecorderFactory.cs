namespace KSTCEngine.GPerf.Recorder
{
    public class GPerfRecorderFactory
    {
        public static IRecorder GetRecorder(RecorderType recorderType)
        {
            IRecorder recorder;
            switch (recorderType)
            {
                case RecorderType.File:
                    recorder = new FileRecorder();
                    break;
                case RecorderType.Console:
                    recorder = new ConsoleRecorder();
                    break;
                case RecorderType.Remote:
                    recorder = new RemoteRecorder();
                    break;
                default:
                    recorder = null;
                    break;
            }

            return recorder;
        }
    }
}
