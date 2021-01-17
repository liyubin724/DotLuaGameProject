using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class CheckerManager
    {
        [MenuItem("Test/CM")]
        static void Test()
        {
            Checker checker = new Checker();

            AndMatchFilter amf = new AndMatchFilter();
            checker.matcher.Filter = amf;

            FileExtensionMatchFilter femf = new FileExtensionMatchFilter()
            {
                ignoreCase = true,
                extension = ".png",
            };
            amf.Add(femf);

            TextureMaxSizeCheckRuler tmscr = new TextureMaxSizeCheckRuler()
            {
                MaxHeight = 512,
                MaxWidth = 512,
            };

            checker.analyser.Add(tmscr);

            TexturePropertyOperationRule tpor = new TexturePropertyOperationRule()
            {
                wrapMode = UnityEngine.TextureWrapMode.Repeat,
            };
            TexturePlatformOperationRule tpmor = new TexturePlatformOperationRule()
            {
                platform = AssetPlatformType.Window
            };
            checker.operater.Add(tpor);
            checker.operater.Add(tpmor);

            string assetPath = "Assets/ArtRes/UI/BG/HighQuality/Alpha/activity_patern1.png";
            if(checker.DoMatch(assetPath))
            {
                int errorCode = 0;
                if (checker.DoAnalyse(assetPath, ref errorCode))
                {
                    checker.DoOperate(assetPath);
                }else
                {
                    Debug.LogError("SSSSSSSS->" + errorCode);
                }
            }
        }
    }
}
