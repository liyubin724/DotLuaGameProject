using System;
using System.Reflection;
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

            string xluaEnvName = "XLua.LuaEnv";
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetType(xluaEnvName, false, false) != null)
                {
                    BuildTargetGroup btg = BuildTargetGroup.Unknown;
#if UNITY_ANDROID
                    btg = BuildTargetGroup.Android;
#elif UNITY_STANDALONE
                    btg = BuildTargetGroup.Standalone;
#endif
                    if (btg != BuildTargetGroup.Unknown)
                    {
                        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
                        if (string.IsNullOrEmpty(symbols))
                        {
                            symbols = "GPERF_XLUA";
                        }
                        else if (symbols.IndexOf("GPERF_XLUA") < 0)
                        {
                            symbols += ";GPERF_XLUA";
                        }
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, symbols);
                    }

                    break;
                }
            }
        }
    }

}
