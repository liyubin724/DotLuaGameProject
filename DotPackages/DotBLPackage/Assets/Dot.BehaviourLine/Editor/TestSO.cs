using DotEngine.BL.Node;
using DotEngine.BL.Node.Action;
using DotEngine.BL.Node.Condition;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestSO
{
    [MenuItem("Test/Seriable")]
    public static void Ser()
    {
        string filePath = "Assets/t.asset";
        NodeData rootData = ScriptableObject.CreateInstance<NodeData>();
        rootData.name = "Root";
        AssetDatabase.CreateAsset(rootData, filePath);


        NodeData childData = ScriptableObject.CreateInstance<ConditionData>();
        childData.name = "Root/Condition Data";
        AssetDatabase.AddObjectToAsset(childData, rootData);

        string filePath2 = "Assets/b.asset";
        NodeData rootData2 = ScriptableObject.CreateInstance<NodeData>();
        rootData2.name = "Root2";
        AssetDatabase.CreateAsset(rootData2, filePath2);

        NodeData childChildData = ScriptableObject.CreateInstance<DurationActionData>();
        childChildData.name = "Root/Condition Data/Event Action Data";
        AssetDatabase.AddObjectToAsset(childChildData, rootData2);

        AssetDatabase.SaveAssets();
    }
}
