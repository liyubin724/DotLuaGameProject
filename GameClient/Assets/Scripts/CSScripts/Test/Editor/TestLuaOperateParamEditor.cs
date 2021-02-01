using DotEngine.Lua;
using DotEngine.Utilities;
using ETest;
using ReflectionMagic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestLuaOperateParam))]
public class TestLuaOperateParamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var ScriptAttributeUtilityDynamic = AssemblyUtility.GetType("ScriptAttributeUtility").AsDynamic();

        //ScriptAttributeUtilityDynamic.GetMethods()
        Array.ForEach((MethodInfo[])ScriptAttributeUtilityDynamic.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), (mInfo) =>
         {
             GUILayout.TextField(mInfo.Name);
         });

        //GUILayout.TextField(ScriptAttributeUtilityDynamic.Name);

        //SerializedProperty p = serializedObject.FindProperty("loParam");
        //GUILayout.TextField(p.type);
        //var propertyHandlerDynamic = ScriptAttributeUtilityDynamic.GetHandler(p).AsDynamic();
        //propertyHandlerDynamic.OnGUILayout(p, "LO Param", false);


    }
}
