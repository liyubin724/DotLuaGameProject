using DotEditor.AAS;
using DotEditor.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestAAS
{
    [MenuItem("Test/AAS/Find Config")]
    public static void TestAAS_FindConfig()
    {
        var configs = AssetDatabaseUtility.FindInstances<AssetBundleGenerateConfig>();
        foreach (var config in configs)
        {
            var datas = config.GetDatas();
            foreach (var data in datas)
            {
                Debug.Log(data.ToString());
            }
        }
    }

    const string k_FolderPath = "Test";
    const string k_TmpPath = "tmp";

}
