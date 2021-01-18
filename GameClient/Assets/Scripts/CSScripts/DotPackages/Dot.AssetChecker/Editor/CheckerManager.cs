using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace DotEditor.AssetChecker
{
    public class CheckerManager
    {
        [MenuItem("Test/CM")]
        static void Test()
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
                wrapMode = UnityEngine.TextureWrapMode.Repeat,
            };
            TexturePlatformOperationRule tpmor = new TexturePlatformOperationRule()
            {
                platform = AssetPlatformType.Window
            };
            checker.operater.Add(tpor);
            checker.operater.Add(tpmor);

            string jsonStr = JsonConvert.SerializeObject(checker, Formatting.Indented,new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText("D:/json.txt", jsonStr);

            Checker nc = JsonConvert.DeserializeObject<Checker>(jsonStr, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            checker = nc;

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
