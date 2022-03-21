using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

namespace DotEditor.UIElements
{
    public static class UIElementsExtension
    {
        public static void SetPosition(this VisualElement self, Vector3 v)
        {
            var c = self.transform.position;
            c = v;
            self.transform.position = c;
        }

        public static void SetRotation(this VisualElement self, Vector3 v)
        {
            var c = self.transform.rotation;
            c = Quaternion.Euler(v);
            self.transform.rotation = c;
        }

        public static void SetScale(this VisualElement self, Vector3 v)
        {
            var c = self.transform.scale;
            c = v;
            self.transform.scale = c;
        }

        public static void SetWidth(this VisualElement self, float v)
        {
            var c = self.style.width;
            c = v;
            self.style.width = c;
        }

        public static void SetHeight(this VisualElement self, float v)
        {
            var c = self.style.height;
            c = v;
            self.style.height = c;
        }

        public static void SetMaxWidth(this VisualElement self, float v)
        {
            var c = self.style.maxWidth;
            c = v;
            self.style.maxWidth = c;
        }

        public static void SetMaxHeight(this VisualElement self, float v)
        {
            var c = self.style.maxHeight;
            c = v;
            self.style.maxHeight = c;
        }

        public static void SetMinWidth(this VisualElement self, float v)
        {
            var c = self.style.minWidth;
            c = v;
            self.style.minWidth = c;
        }

        public static void SetMinHeight(this VisualElement self, float v)
        {
            var c = self.style.minHeight;
            c = v;
            self.style.minHeight = c;
        }

        public static void SetFlexBasis(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.flexBasis;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.flexBasis = c;
        }

        public static void SetFlexGrow(this VisualElement self, float v)
        {
            var c = self.style.flexGrow;
            c.value = v;
            self.style.flexGrow = c;
        }

        public static void SetFlexShrink(this VisualElement self, float v)
        {
            var c = self.style.flexShrink;
            c.value = v;
            self.style.flexShrink = c;
        }

        public static void SetFlexDirection(this VisualElement self, FlexDirection v)
        {
            var c = self.style.flexDirection;
            c.value = v;
            self.style.flexDirection = c;
        }

        public static void SetFlexWarp(this VisualElement self, Wrap v)
        {
            var c = self.style.flexWrap;
            c.value = v;
            self.style.flexWrap = c;
        }

        public static void SetPositionLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.left;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.left = c;
        }

        public static void SetPositionTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.top;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.top = c;
        }

        public static void SetPositionRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.right;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.right = c;
        }

        public static void SetPositionBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.bottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.bottom = c;
        }

        public static void SetMarginLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginLeft;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginLeft = c;
        }

        public static void SetMarginTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginTop;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginTop = c;
        }

        public static void SetMarginRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginRight;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginRight = c;
        }

        public static void SetMarginBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginBottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginBottom = c;
        }

        public static void SetPaddingLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingLeft;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingLeft = c;
        }

        public static void SetPaddingTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingTop;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingTop = c;
        }

        public static void SetPaddingRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingRight;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingRight = c;
        }

        public static void SetPaddingBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingBottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingBottom = c;
        }

        public static void SetPosition(this VisualElement self, Position v)
        {
            var c = self.style.position;
            c.value = v;
            self.style.position = c;
        }

        public static void SetAlignSelf(this VisualElement self, Align v)
        {
            var c = self.style.alignSelf;
            c.value = v;
            self.style.alignSelf = c;
        }

        public static void SetTextAlign(this VisualElement self, TextAnchor v)
        {
            var c = self.style.unityTextAlign;
            c.value = v;
            self.style.unityTextAlign = c;
        }

        public static void SetFontStyle(this VisualElement self, FontStyle v)
        {
            var c = self.style.unityFontStyleAndWeight;
            c.value = v;
            self.style.unityFontStyleAndWeight = c;
        }

        public static void SetFontSize(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.fontSize;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.fontSize = c;
        }

        public static void SetFontSize(float v, LengthUnit v2)
        {
            var ve = new VisualElement();
            var c = ve.style.fontSize;
            var c2 = c.value;
            c2.value = 12;
            c2.unit = LengthUnit.Percent;
            c.value = c2;
            ve.style.fontSize = c;
        }

        public static void SetTextWrap(this VisualElement self, WhiteSpace v)
        {
            var c = self.style.whiteSpace;
            c.value = v;
            self.style.whiteSpace = c;
        }

        public static void SetTextColor(this VisualElement self, Color v)
        {
            var c = self.style.color;
            c.value = v;
            self.style.color = c;
        }

        public static void SetBackgroundColor(this VisualElement self, Color v)
        {
            var c = self.style.backgroundColor;
            c.value = v;
            self.style.backgroundColor = c;
        }

        public static void SetBorderColor(this VisualElement self, Color v)
        {
            self.SetBorderLeftColor(v);
            self.SetBorderTopColor(v);
            self.SetBorderRightColor(v);
            self.SetBorderBottomColor(v);
        }

        public static void SetBorderLeftColor(this VisualElement self, Color v)
        {
            var c = self.style.borderLeftColor;
            c.value = v;
            self.style.borderLeftColor = c;
        }

        public static void SetBorderTopColor(this VisualElement self, Color v)
        {
            var c = self.style.borderTopColor;
            c.value = v;
            self.style.borderTopColor = c;
        }

        public static void SetBorderRightColor(this VisualElement self, Color v)
        {
            var c = self.style.borderRightColor;
            c.value = v;
            self.style.borderRightColor = c;
        }

        public static void SetBorderBottomColor(this VisualElement self, Color v)
        {
            var c = self.style.borderBottomColor;
            c.value = v;
            self.style.borderBottomColor = c;
        }

        public static void SetFont(this VisualElement self, Font v)
        {
            var c = self.style.unityFont;
            c.value = v;
            self.style.unityFont = c;
        }

        public static void SetBackgroundScaleMode(this VisualElement self, ScaleMode v)
        {
            var c = self.style.unityBackgroundScaleMode;
            c.value = v;
            self.style.unityBackgroundScaleMode = c;
        }

        public static void SetBackgroundImageColor(this VisualElement self, Color v)
        {
            var c = self.style.unityBackgroundImageTintColor;
            c.value = v;
            self.style.unityBackgroundImageTintColor = c;
        }

        public static void SetAlignItems(this VisualElement self, Align v)
        {
            var c = self.style.alignItems;
            c.value = v;
            self.style.alignItems = c;
        }

        public static void SetAlignContent(this VisualElement self, Align v)
        {
            var c = self.style.alignContent;
            c.value = v;
            self.style.alignContent = c;
        }

        public static void SetJustifyContent(this VisualElement self, Justify v)
        {
            var c = self.style.justifyContent;
            c.value = v;
            self.style.justifyContent = c;
        }

        public static void SetBorderWidth(this VisualElement self, float v)
        {
            self.SetBorderLeftWidth(v);
            self.SetBorderTopWidth(v);
            self.SetBorderRightWidth(v);
            self.SetBorderBottomWidth(v);
        }

        public static void SetBorderLeftWidth(this VisualElement self, float v)
        {
            var c = self.style.borderLeftWidth;
            c.value = v;
            self.style.borderLeftWidth = c;
        }

        public static void SetBorderRightWidth(this VisualElement self, float v)
        {
            var c = self.style.borderRightWidth;
            c.value = v;
            self.style.borderRightWidth = c;
        }

        public static void SetBorderTopWidth(this VisualElement self, float v)
        {
            var c = self.style.borderTopWidth;
            c.value = v;
            self.style.borderTopWidth = c;
        }

        public static void SetBorderBottomWidth(this VisualElement self, float v)
        {
            var c = self.style.borderBottomWidth;
            c.value = v;
            self.style.borderBottomWidth = c;
        }

        public static void SetBorderRadius(this VisualElement self, float v)
        {
            self.SetBorderTopLeftRadius(v);
            self.SetBorderTopRightRadius(v);
            self.SetBorderBottomLeftRadius(v);
            self.SetBorderBottomRightRadius(v);
        }

        public static void SetBorderTopLeftRadius(this VisualElement self, float v)
        {
            var c = self.style.borderTopLeftRadius;
            c.value = v;
            self.style.borderTopLeftRadius = c;
        }

        public static void SetBorderTopRightRadius(this VisualElement self, float v)
        {
            var c = self.style.borderTopRightRadius;
            c.value = v;
            self.style.borderTopRightRadius = c;
        }

        public static void SetBorderBottomLeftRadius(this VisualElement self, float v)
        {
            var c = self.style.borderBottomLeftRadius;
            c.value = v;
            self.style.borderBottomLeftRadius = c;
        }

        public static void SetBorderBottomRightRadius(this VisualElement self, float v)
        {
            var c = self.style.borderBottomRightRadius;
            c.value = v;
            self.style.borderBottomRightRadius = c;
        }

        public static void SetSlide(this VisualElement self, int v)
        {
            self.SetSliceLeft(v);
            self.SetSliceRight(v);
            self.SetSliceTop(v);
            self.SetSliceBottom(v);
        }

        public static void SetSliceLeft(this VisualElement self, int v)
        {
            var c = self.style.unitySliceLeft;
            c.value = v;
            self.style.unitySliceLeft = c;
        }

        public static void SetSliceRight(this VisualElement self, int v)
        {
            var c = self.style.unitySliceRight;
            c.value = v;
            self.style.unitySliceRight = c;
        }

        public static void SetSliceTop(this VisualElement self, int v)
        {
            var c = self.style.unitySliceTop;
            c.value = v;
            self.style.unitySliceTop = c;
        }

        public static void SetSliceBottom(this VisualElement self, int v)
        {
            var c = self.style.unitySliceBottom;
            c.value = v;
            self.style.unitySliceBottom = c;
        }

        public static void SetOpacity(this VisualElement self, float v)
        {
            var c = self.style.opacity;
            c.value = v;
            self.style.opacity = c;
        }

        public static void SetVisibility(this VisualElement self, Visibility v)
        {
            var c = self.style.visibility;
            c.value = v;
            self.style.visibility = c;
        }

        public static void SetDisplay(this VisualElement self, DisplayStyle v)
        {
            var c = self.style.display;
            c.value = v;
            self.style.display = c;
        }

        public static void SetOverflow(this VisualElement self, Overflow v)
        {
            var c = self.style.overflow;
            c.value = v;
            self.style.overflow = c;
        }
    }
}
