using DotEditor.GUIExt.NativeDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class AssetCheckerWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Asset Checker")]
        static void ShowWin()
        {
            var win = GetWindow<AssetCheckerWindow>();
            win.titleContent = new GUIContent("Asset Checker");
            win.Show();
        }

        private Checker checker = null;
        private NObjectDrawer drawer = null;
        private void OnEnable()
        {
            checker = CreateTestChecker();
            drawer = new NObjectDrawer(checker)
            {
                IsShowBox = true,
                IsShowInherit = false,
                IsShowScroll = true,
            };
        }
        private void OnGUI()
        {
            drawer.OnGUILayout();
        }


        static Checker CreateTestChecker()
        {
            Checker checker = new Checker();
            FileExtensionMatchFilter femf = new FileExtensionMatchFilter()
            {
                ignoreCase = true,
                extension = ".png",
            };
            checker.matcher.Add(femf);

            TextureMaxSizeCheckRule tmscr = new TextureMaxSizeCheckRule()
            {
                maxHeight = 512,
                maxWidth = 512,
            };

            checker.analyser.Add(tmscr);

            TexturePropertyOperationRule tpor = new TexturePropertyOperationRule()
            {
                wrapMode = TextureWrapMode.Repeat,
            };
            TexturePlatformOperationRule tpmor = new TexturePlatformOperationRule()
            {
                platform = AssetPlatformType.Window
            };
            checker.operater.Add(tpor);
            checker.operater.Add(tpmor);

            return checker;
        }

        [MenuItem("Test/CM")]
        static void Test()
        {
            Checker checker = CreateTestChecker();
            string assetPath = "Assets/ArtRes/UI/BG/HighQuality/Alpha/activity_patern1.png";
            if (checker.DoMatch(assetPath))
            {
                int errorCode = 0;
                if (checker.DoAnalyse(assetPath, ref errorCode))
                {
                    checker.DoOperate(assetPath);
                }
                else
                {
                    Debug.LogError("SSSSSSSS->" + errorCode);
                }
            }
        }
    }
}
