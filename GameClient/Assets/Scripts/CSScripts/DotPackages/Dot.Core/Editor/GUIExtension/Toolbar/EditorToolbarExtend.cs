//#define EDITOR_TOOLBAR_GIZMO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension.Toolbar
{
    public enum EditorToolbarOrientation
    {
        Left,
        Right,
    }

    public class EditorToolbarGroup
    {
        internal EditorToolbarItem[] Items { get; private set; }

        public EditorToolbarGroup(EditorToolbarButton[] btns)
        {
            Items = btns;
        }

        internal bool IsToggleGroup { get; private set; }
        public EditorToolbarGroup(EditorToolbarToggleButton[] btns)
        {
            Items = btns;
            IsToggleGroup = true;
            foreach(var btn in btns)
            {
                btn.OnSelectedAction = OnSelectedChanged;
            }
        }

        private void OnSelectedChanged(EditorToolbarToggleButton btn)
        {
            foreach(var item in Items)
            {
                if(item !=btn)
                {
                    (item as EditorToolbarToggleButton).IsSelected = false;
                }
            }
        }

        internal static EditorToolbarGroup CreateGroup(EditorToolbarItem item)
        {
            if(item == null)
            {
                return null;
            }

            if(item.GetType() == typeof(EditorToolbarButton))
            {
                return new EditorToolbarGroup(new EditorToolbarButton[] { item as EditorToolbarButton });
            }else if(item .GetType() == typeof(EditorToolbarToggleButton))
            {
                return new EditorToolbarGroup(new EditorToolbarToggleButton[] { item as EditorToolbarToggleButton });
            }

            return null;
        }
    }

    [InitializeOnLoad]
    public static class EditorToolbarExtend
    {
        private static readonly Type containterType = typeof(IMGUIContainer);
        private static readonly Type toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static readonly Type guiViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GUIView");

        private static readonly FieldInfo onGuiHandler = containterType.GetField("m_OnGUIHandler",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly PropertyInfo visualTree = guiViewType.GetProperty("visualTree",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static UnityObject toolbar;
        static EditorToolbarExtend()
        {
            EditorApplication.update -= DoUpdate;
            EditorApplication.update += DoUpdate;
        }

        private static void DoUpdate()
        {
            if (toolbar != null) return;
            var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
            if (toolbars == null || toolbars.Length == 0) return;

            toolbar = toolbars[0];

            var container = (visualTree.GetValue(toolbar, null) as VisualElement)[0];

            var handler = onGuiHandler.GetValue(container) as Action;
            handler -= OnGUIHandler;
            handler += OnGUIHandler;
            onGuiHandler.SetValue(container, handler);
        }

        private static void OnGUIHandler()
        {
            var screenWidth = EditorGUIUtility.currentViewWidth;

            var leftRect = new Rect(2, 0, screenWidth, Styles.TOOLBAR_HEIGHT);
            leftRect.y += Styles.PADDING;
            leftRect.xMax = (screenWidth - Styles.LEFT_FROM_STRIP_OFFSET_X) / 2;
            leftRect.xMin += Styles.LEFT_FROM_TOOLS_OFFSET_X;
            leftRect.xMin += Styles.SPACEING;
            leftRect.xMax -= Styles.SPACEING;

            if (leftRect.width > 0)
            {
                DrawerToolbar(leftRect, EditorToolbarOrientation.Left, leftGroups);
            }

            var rightRect = new Rect(0, 0, screenWidth, Styles.TOOLBAR_HEIGHT);
            rightRect.y += Styles.PADDING;
            rightRect.xMin = (screenWidth + Styles.RIGHT_FROM_STRIP_OFFSET_X) / 2;
            rightRect.xMin += Styles.SPACEING;
            rightRect.xMax = screenWidth - Styles.RIGHT_FROM_TOOLS_OFFSET_X;
            rightRect.xMax -= Styles.SPACEING;

            if (rightRect.width > 0)
            {
                DrawerToolbar(rightRect, EditorToolbarOrientation.Right, rightGroups);
            }
        }

        private static List<EditorToolbarGroup> leftGroups = new List<EditorToolbarGroup>();
        private static List<EditorToolbarGroup> rightGroups = new List<EditorToolbarGroup>();

        private static float GetItemWidth(EditorToolbarItem item)
        {
            float itemWidth = item.GetItemWidth();
            if(item.GetType() == typeof(EditorToolbarButton) && itemWidth <Styles.STAND_BUTTON_WIDTH)
            {
                itemWidth = Styles.STAND_BUTTON_WIDTH;
            }
            return itemWidth;
        }

        private static float GetGroupWidth(EditorToolbarGroup itemGroup)
        {
            return (from item in itemGroup.Items select GetItemWidth(item)).ToArray().Sum();
        }

        private static void DrawerToolbar(Rect rect, EditorToolbarOrientation orientation, List<EditorToolbarGroup> itemGroups)
        {
            if (itemGroups.Count == 0) return;

#if EDITOR_TOOLBAR_GIZMO
            DEGUI.DrawAreaLine(rect, Color.blue);
#endif

            float groupStartX = rect.x;
            if(orientation == EditorToolbarOrientation.Left)
            {
                groupStartX = rect.x + rect.width;
            }
            foreach (var itemGroup in itemGroups)
            {
                float groupWidth = GetGroupWidth(itemGroup);
                Rect groupRect = new Rect(groupStartX, rect.y, groupWidth, rect.height);
                if (orientation == EditorToolbarOrientation.Left)
                {
                    groupStartX -= groupWidth;
                    groupRect.x = groupStartX;
                }else if(orientation == EditorToolbarOrientation.Right)
                {
                    groupStartX += groupWidth;
                }

#if EDITOR_TOOLBAR_GIZMO
                DEGUI.DrawAreaLine(groupRect, Color.yellow);
#endif

                float itemStartX = groupRect.x;
                if(orientation == EditorToolbarOrientation.Left)
                {
                    itemStartX = groupRect.x + groupRect.width;
                }
                for(int i =0;i<itemGroup.Items.Length;++i)
                {
                    var item = itemGroup.Items[i];
                    float itemWidth = item.GetItemWidth();
                    Rect itemRect = new Rect(itemStartX, groupRect.y, itemWidth, groupRect.height);
                    if (orientation == EditorToolbarOrientation.Left)
                    {
                        itemStartX -= itemWidth;
                        itemRect.x = itemStartX;
                    }
                    else if (orientation == EditorToolbarOrientation.Right)
                    {
                        itemStartX += itemWidth;
                    }

#if EDITOR_TOOLBAR_GIZMO
                    DEGUI.DrawAreaLine(itemRect, Color.green);
#endif

                    GUIStyle style = null;
                    if (item.GetType() == typeof(EditorToolbarButton))
                    {
                        if (itemGroup.Items.Length == 1)
                        {
                            style = Styles.commandStyle;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (orientation == EditorToolbarOrientation.Left)
                                {
                                    style = Styles.commandRightStyle;
                                }
                                else if (orientation == EditorToolbarOrientation.Right)
                                {
                                    style = Styles.commandLeftStyle;
                                }
                            }
                            else if (i == itemGroup.Items.Length - 1)
                            {
                                if (orientation == EditorToolbarOrientation.Left)
                                {
                                    style = Styles.commandLeftStyle;
                                }
                                else if (orientation == EditorToolbarOrientation.Right)
                                {
                                    style = Styles.commandRightStyle;
                                }
                            }
                            else
                            {
                                style = Styles.commandMidStyle;
                            }
                        }
                    }
                    item.OnItemGUI(itemRect, style);
                }

                if (orientation == EditorToolbarOrientation.Left)
                {
                    groupStartX -= Styles.ITEM_GROUP_SPACE;
                }
                else if (orientation == EditorToolbarOrientation.Right)
                {
                    groupStartX += Styles.ITEM_GROUP_SPACE;
                }
            }
        }

        public static void AddItemGroup(EditorToolbarGroup itemGroup, EditorToolbarOrientation orientation)
        {
            if (orientation == EditorToolbarOrientation.Left)
            {
                leftGroups.Add(itemGroup);
            }
            else if (orientation == EditorToolbarOrientation.Right)
            {
                rightGroups.Add(itemGroup);
            }
        }

        public static void AddItem(EditorToolbarItem item, EditorToolbarOrientation orientation)
        {
            if (orientation == EditorToolbarOrientation.Left)
            {
                leftGroups.Add(EditorToolbarGroup.CreateGroup(item));
            }
            else if (orientation == EditorToolbarOrientation.Right)
            {
                rightGroups.Add(EditorToolbarGroup.CreateGroup(item));
            }
        }

        public static void RemoveItem(EditorToolbarItem item, EditorToolbarOrientation orientation)
        {

        }

        public static void ClearAll(EditorToolbarOrientation orientation)
        {
            if(orientation == EditorToolbarOrientation.Left)
            {
                leftGroups.Clear();
            }else if(orientation == EditorToolbarOrientation.Right)
            {
                rightGroups.Clear();
            }
        }

        private class Styles
        {
            internal static readonly float SPACEING = 10;
            internal static readonly float PADDING = 5;
            internal static readonly float TOOLBAR_HEIGHT = 24;

            internal static readonly float LEFT_FROM_TOOLS_OFFSET_X = 400.0f;
            internal static readonly float LEFT_FROM_STRIP_OFFSET_X = 140.0f;
            internal static readonly float RIGHT_FROM_TOOLS_OFFSET_X = 400.0f;
            internal static readonly float RIGHT_FROM_STRIP_OFFSET_X = 36.0f;

            internal static readonly float ITEM_GROUP_SPACE = 10.0f;

            internal static readonly float STAND_BUTTON_WIDTH = 32.0f;

            internal static readonly GUIStyle commandStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandMidStyle = new GUIStyle("CommandMid")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandLeftStyle = new GUIStyle("CommandLeft")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
            internal static readonly GUIStyle commandRightStyle = new GUIStyle("CommandRight")
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove
            };
        }
    }
}
