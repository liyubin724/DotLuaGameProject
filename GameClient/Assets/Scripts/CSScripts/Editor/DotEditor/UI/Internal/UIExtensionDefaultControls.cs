using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using System.IO;
using DotEngine.UI;
using DotEngine.Lua.UI;
using DotEngine.Lua.UI.Handler;

namespace DotEditor.UI
{
    internal static class UIExtensionDefaultControls
    {
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        // Helper methods at top

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        static GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = s_TextColor;

            System.Type textType = lbl.GetType();
            MethodInfo mi = textType.GetMethod("AssignDefaultFont", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(lbl, new System.Object[] { });
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        public static GameObject CreateAtlasImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("AtlasImage", s_ImageElementSize);
            go.AddComponent<UIAtlasImage>();
            return go;
        }

        public static GameObject CreateAtlasImageAnimation(Resources resources)
        {
            GameObject go = CreateUIElementRoot("AtlasImageAnimation", s_ImageElementSize);
            go.AddComponent<UIAtlasImageAnimation>();
            return go;
        }

        public static GameObject CreateRawImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("RawImage", s_ImageElementSize);
            go.AddComponent<UIRawImage>();
            return go;
        }

        public static GameObject CreateBitmapText(Resources resources)
        {
            GameObject go = CreateUIElementRoot("BitmapText", s_ImageElementSize);
            go.AddComponent<BitmapFontUIText>();
            return go;
        }

        public static GameObject CreateLuaButton<T>(Resources resources) where T:Image
        {
            GameObject buttonRoot = CreateUIElementRoot("LuaButton", s_ThickElementSize);

            GameObject childText = new GameObject("Text");
            childText.AddComponent<RectTransform>();
            SetParentAndAlign(childText, buttonRoot);

            T image = buttonRoot.AddComponent<T>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            Button bt = buttonRoot.AddComponent<Button>();
            SetDefaultColorTransitionValues(bt);

            LuaButtonHandler handler = buttonRoot.AddComponent<LuaButtonHandler>();
            handler.button = bt;

            Text text = childText.AddComponent<Text>();
            text.text = "Lua Button";
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            return buttonRoot;
        }

        public static GameObject CreateInputField(Resources resources)
        {
            GameObject root = CreateUIElementRoot("LuaInputField", s_ThickElementSize);

            GameObject childPlaceholder = CreateUIObject("Placeholder", root);
            GameObject childText = CreateUIObject("Text", root);

            Image image = root.AddComponent<Image>();
            image.sprite = resources.inputField;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            InputField inputField = root.AddComponent<InputField>();
            SetDefaultColorTransitionValues(inputField);

            LuaInputFieldHandler handler = root.AddComponent<LuaInputFieldHandler>();
            handler.inputField = inputField;

            Text text = childText.AddComponent<Text>();
            text.text = "";
            text.supportRichText = false;
            SetDefaultTextValues(text);

            Text placeholder = childPlaceholder.AddComponent<Text>();
            placeholder.text = "Enter text...";
            placeholder.fontStyle = FontStyle.Italic;
            // Make placeholder color half as opaque as normal text color.
            Color placeholderColor = text.color;
            placeholderColor.a *= 0.5f;
            placeholder.color = placeholderColor;

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.offsetMin = new Vector2(10, 6);
            textRectTransform.offsetMax = new Vector2(-10, -7);

            RectTransform placeholderRectTransform = childPlaceholder.GetComponent<RectTransform>();
            placeholderRectTransform.anchorMin = Vector2.zero;
            placeholderRectTransform.anchorMax = Vector2.one;
            placeholderRectTransform.sizeDelta = Vector2.zero;
            placeholderRectTransform.offsetMin = new Vector2(10, 6);
            placeholderRectTransform.offsetMax = new Vector2(-10, -7);

            inputField.textComponent = text;
            inputField.placeholder = placeholder;

            return root;
        }
    }
}
