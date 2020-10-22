using UnityEditor;

namespace KSTCEditor.GPerf
{
    public class GPerfSetting
    {
        [InitializeOnLoadMethod]
        private static void OnInited()
        {
            if(!PlayerSettings.enableFrameTimingStats)
            {
                PlayerSettings.enableFrameTimingStats = true;
            }
        }
    }

}
