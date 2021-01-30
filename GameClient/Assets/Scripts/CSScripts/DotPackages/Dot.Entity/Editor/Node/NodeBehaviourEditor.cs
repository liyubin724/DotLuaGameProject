using DotEditor.GUIExtension;
using DotEngine.Entity.Node;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    [CustomEditor(typeof(NodeBehaviour))]
    public class NodeBehaviourEditor : Editor
    {
        private NodeBehaviour nodeBehaviour = null;

        private Vector2 scrollPos = Vector2.zero;

        private List<NodeData> bindNodes = null;
        private List<NodeData> boneNodes = null;
        private List<NodeData> smRendererNodes = null;
        private ReorderableList bindNodeRList = null;
        private ReorderableList boneNodeRList = null;
        private ReorderableList smRendererNodeRList = null;

        private bool isBindNodeFoldout = false;
        private bool isBoneNodeFoldout = true;
        private bool isRendererNodeFoldout = true;

        void OnEnable()
        {
            nodeBehaviour = (NodeBehaviour)target;
            bindNodes = new List<NodeData>(nodeBehaviour.bindNodes);
            boneNodes = new List<NodeData>(nodeBehaviour.boneNodes);
            smRendererNodes = new List<NodeData>(nodeBehaviour.smRendererNodes);
        }

        private ReorderableList CreateRList(GUIContent titleContent,NodeType nodeType,List<NodeData> datas)
        {
            bool isEditable = nodeType == NodeType.BindNode;
            ReorderableList reorderableList = new ReorderableList(datas, typeof(NodeData), isEditable, true, isEditable, isEditable);
            reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 3;
            reorderableList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, titleContent,EGUIStyles.BoldLabelStyle);
            };
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                Rect indexRect = new Rect(rect);
                indexRect.width = Styles.IndexRectWidth;
                EditorGUI.LabelField(indexRect, "" + index);

                Rect contentRect = new Rect(rect.x + Styles.IndexRectWidth, rect.y, 
                                                            rect.width - Styles.IndexRectWidth - Styles.CheckRectWidth, EditorGUIUtility.singleLineHeight);
                NodeData data = datas[index];
                if(nodeType == NodeType.BindNode)
                {
                    data.name = EditorGUI.TextField(contentRect, Contents.NameContent, data.name);
                }
                else
                {
                    EditorGUI.TextField(contentRect, Contents.NameContent, data.name);
                }

                contentRect.y += contentRect.height;
                if (nodeType == NodeType.SMRendererNode)
                {
                    EditorGUI.ObjectField(contentRect, Contents.RendererContent, data.renderer,typeof(SkinnedMeshRenderer),true);
                } else if (nodeType == NodeType.BoneNode)
                {
                    EditorGUI.ObjectField(contentRect, Contents.TransformContent, data.transform, typeof(Transform), true);
                } else if (nodeType == NodeType.BindNode)
                {
                    data.transform = (Transform)EditorGUI.ObjectField(contentRect, Contents.TransformContent, data.transform, typeof(Transform), true);
                }

                Rect checkRect = new Rect(contentRect.x + contentRect.width, rect.y, Styles.CheckRectWidth, Styles.CheckRectWidth);
                string errorTips = string.Empty;
                bool isValid = (from d in datas where d.name == data.name select d).ToArray().Length == 1;
                if(isValid)
                {
                    isValid = (nodeType == NodeType.SMRendererNode) ? data.renderer != null : data.transform != null;
                    if(!isValid)
                    {
                        errorTips = Contents.ContentIsNull;
                    }
                }else
                {
                    errorTips = Contents.NameRepeatStr;
                }
                if(!isValid)
                {
                    if(GUI.Button(checkRect,new GUIContent(EGUIResources.ErrorIcon,errorTips)))
                    {
                        EditorUtility.DisplayDialog("Error", errorTips, "OK");
                    }
                }

                contentRect.y += contentRect.height;
                EGUI.DrawHorizontalLine(contentRect);
            };

            reorderableList.onAddCallback = (list) =>
            {
                NodeData data = new NodeData()
                {
                    nodeType = nodeType,
                };
                datas.Add(data);
            };

            return reorderableList;
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(nodeBehaviour);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                isBindNodeFoldout = EditorGUILayout.Foldout(isBindNodeFoldout, Contents.BindTitleContent, true);
                if(!isBindNodeFoldout)
                {
                    if(bindNodeRList == null)
                    {
                        bindNodeRList = CreateRList(Contents.BindTitleContent, NodeType.BindNode, bindNodes);
                    }
                    bindNodeRList.DoLayoutList();
                }

                isBoneNodeFoldout = EditorGUILayout.Foldout(isBoneNodeFoldout, Contents.BoneTitleContent, true);
                if(!isBoneNodeFoldout)
                {
                    if(boneNodeRList == null)
                    {
                        boneNodeRList = CreateRList(Contents.BoneTitleContent, NodeType.BoneNode, boneNodes);
                    }
                    boneNodeRList.DoLayoutList();
                }

                isRendererNodeFoldout = EditorGUILayout.Foldout(isRendererNodeFoldout, Contents.RendererTitleContent,true);
                if (!isRendererNodeFoldout)
                {
                    if (smRendererNodeRList == null)
                    {
                        smRendererNodeRList = CreateRList(Contents.RendererTitleContent, NodeType.SMRendererNode, smRendererNodes);
                    }
                    smRendererNodeRList.DoLayoutList();
                }
            }
            EditorGUILayout.EndScrollView();
            if (GUI.changed)
            {
                nodeBehaviour.bindNodes = bindNodes.ToArray();
                nodeBehaviour.boneNodes = boneNodes.ToArray();
                nodeBehaviour.smRendererNodes = smRendererNodes.ToArray();

                EditorUtility.SetDirty(target);
            }
        }

        class Contents
        {
            public static GUIContent BindTitleContent = new GUIContent("Bind Node");
            public static GUIContent BoneTitleContent = new GUIContent("Bone Node");
            public static GUIContent RendererTitleContent = new GUIContent("Renderer Node");

            public static string NameRepeatStr = "the name of data is repeated!";
            public static string ContentIsNull = "the content of data is empty";

            public static GUIContent NameContent = new GUIContent("Name");
            public static GUIContent TransformContent = new GUIContent("Transform");
            public static GUIContent RendererContent = new GUIContent("Renderer");
        }
        
        class Styles
        {
            public static float IndexRectWidth = 30;
            public static float CheckRectWidth = 30;
            public static GUIStyle centerLabelStyle = null;

            static Styles()
            {
                centerLabelStyle = EGUIStyles.MiddleCenterLabel;
            }
        }

    }
}
