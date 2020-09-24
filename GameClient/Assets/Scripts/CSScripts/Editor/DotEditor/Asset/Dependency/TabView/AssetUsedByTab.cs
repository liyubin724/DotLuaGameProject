using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Dependency
{
    internal class AssetUsedByTab : AAssetDependencyTab
    {
        private AllAssetDependencyData m_AllAssetData = null;
        public AssetUsedByTab(EditorWindow win) : base(win)
        {
        }

        public override void OnEnable()
        {
            m_AllAssetData = AssetDependencyUtil.GetOrCreateAllAssetData();
            base.OnEnable();
        }

        protected override void OnAssetSelectionChanged(string assetPath)
        {
            AssetDependencyData[] datas = AssetDependencyUtil.GetAssetUsedBy(assetPath, (title, message, progress) =>
            {
                EditorUtility.DisplayProgressBar(title, message, progress);
            });
            EditorUtility.ClearProgressBar();

            string[] usedDatas = (from data in datas select data.assetPath).ToArray();
            treeView?.ShowDependency(usedDatas, new string[] { assetPath });
        }

        protected override void DrawSelectedAsset()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.ObjectField("All Asset Data", m_AllAssetData, typeof(AllAssetDependencyData), false);
                if (GUILayout.Button("Reload", GUILayout.Width(60)))
                {
                    if (EditorUtility.DisplayDialog("Warning", "This will take a lot of time.Are you sure?", "OK", "Cancel"))
                    {
                        AssetDependencyUtil.FindAllAssetData((title,message,progress)=>
                        {
                            EditorUtility.DisplayProgressBar(title, message, progress);
                        });
                        EditorUtility.ClearProgressBar();

                        if (!EditorUtility.IsPersistent(m_AllAssetData) && EditorUtility.DisplayDialog("Save As", "Do you want to save it?", "OK"))
                        {
                            string filePath = EditorUtility.SaveFilePanelInProject("save", "all_asset_dependency", "asset", "Save data as a asset");
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                AssetDatabase.CreateAsset(m_AllAssetData, filePath);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                base.DrawSelectedAsset();
                if(GUILayout.Button("Selected",GUILayout.Width(60)))
                {
                    treeView.RefreshDependency();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
