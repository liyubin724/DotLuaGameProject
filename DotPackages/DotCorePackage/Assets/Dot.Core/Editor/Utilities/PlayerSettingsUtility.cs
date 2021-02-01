using UnityEditor;

namespace DotEditor.Utilities
{
    public static class PlayerSettingsUtility
    {
        public static bool HasScriptingDefineSymbol(string symbol)
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(ActiveBuildTargetGroup);
            return symbols.IndexOf(symbol) >= 0;
        }

        public static void AddScriptingDefineSymbol(string symbol)
        {
            BuildTargetGroup btGroup = ActiveBuildTargetGroup;
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(btGroup);
            int symbolIndex = symbols.IndexOf(symbol);
            if(symbolIndex>=0)
            {
                return;
            }

            if (symbols.Length > 0 && symbols[symbols.Length - 1] != ';')
            {
                symbols += ";";
            }
            symbols += symbol;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(btGroup, symbols);
        }

        public static void RemoveScriptingDefineSymbol(string symbol)
        {
            BuildTargetGroup btGroup = ActiveBuildTargetGroup;
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(btGroup);
            int symbolIndex = symbols.IndexOf(symbol);
            if (symbolIndex < 0)
            {
                return;
            }

            if (symbols.Length > symbol.Length && symbols[symbolIndex + 1] == ';')
            {
                symbols = symbols.Replace(symbol + ";", "");
            }
            else
            {
                symbols = symbols.Replace(symbol, "");
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(btGroup, symbols);
        }

        private static BuildTargetGroup ActiveBuildTargetGroup
        {
            get
            {
                BuildTargetGroup btGroup = BuildTargetGroup.Standalone;
                BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
                if (buildTarget == BuildTarget.StandaloneWindows64)
                {
                    btGroup = BuildTargetGroup.Standalone;
                }
                else if (buildTarget == BuildTarget.XboxOne)
                {
                    btGroup = BuildTargetGroup.XboxOne;
                }
                else if (buildTarget == BuildTarget.PS4)
                {
                    btGroup = BuildTargetGroup.PS4;
                }else if(buildTarget == BuildTarget.Android)
                {
                    btGroup = BuildTargetGroup.Android;
                }else if(buildTarget == BuildTarget.iOS)
                {
                    btGroup = BuildTargetGroup.iOS;
                }

                return btGroup;
            }
        }
    }
}
