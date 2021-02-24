using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using DotEditor.GUIExt.NativeDrawer;
using DotEditor.GUIExt;
using UnityEngine;
using DotEditor.AAS.Matchers;

namespace DotEditor.AAS.Reprocessor
{
    [CustomEditor(typeof(AssetReprocessor))]
    public class AssetReprocessorEditor :Editor
    {
        private AssetReprocessor reprocessor = null;

        private ArrayDrawer matcherDrawer = null;
        private ArrayDrawer reprocessDrawer = null;
        private void OnEnable()
        {
            reprocessor = target as AssetReprocessor;
            matcherDrawer = new ArrayDrawer(reprocessor.matcher.matchers)
            {
                Header = "Matchers",
                IsShowScroll = false,
                IsShowBox = true,
                IsShowTargetType = true,
                CreateNewItem = () =>
                {
                    AssetMatcherUtility.ShowMenu((matcher) =>
                    {
                        reprocessor.matcher.matchers.Add(matcher);
                        matcherDrawer.Refresh();
                    });
                }
            };
            reprocessDrawer = new ArrayDrawer(reprocessor.reprocess.reprocesses)
            {
                Header = "Reprocesses",
                IsShowScroll = false,
                IsShowBox = true,
                IsShowTargetType = true,
                CreateNewItem = () =>
                {
                    AssetReprocessUtility.ShowMenu((reprocess) =>
                    {
                        reprocessor.reprocess.reprocesses.Add(reprocess);
                        reprocessDrawer.Refresh();
                    });
                }
            };
        }

        Vector2 scrollPos;
        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            EditorGUILayout.Space();

            reprocessor.enable = EditorGUILayout.Toggle("Enable", reprocessor.enable);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                matcherDrawer.OnGUILayout();
                reprocessDrawer.OnGUILayout();
            }
            EditorGUILayout.EndScrollView();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
