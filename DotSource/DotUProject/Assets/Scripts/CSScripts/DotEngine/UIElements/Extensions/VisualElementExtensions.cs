using System;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

namespace DotEngine.UIElements
{
    public static class VisualElementExtensions
    {
        public static VisualElement SetClass(this VisualElement self, params string[] classNames)
        {
            if (classNames != null && classNames.Length > 0)
            {
                foreach (var className in classNames)
                {
                    self.AddToClassList(className);
                }
            }

            return self;
        }

        public static VisualElement SetPosition(this VisualElement self, Vector3 v)
        {
            var c = self.transform.position;
            c = v;
            self.transform.position = c;

            return self;
        }

        public static VisualElement SetRotation(this VisualElement self, Vector3 v)
        {
            var c = self.transform.rotation;
            c = Quaternion.Euler(v);
            self.transform.rotation = c;

            return self;
        }

        public static VisualElement SetScale(this VisualElement self, Vector3 v)
        {
            var c = self.transform.scale;
            c = v;
            self.transform.scale = c;

            return self;
        }

        public static VisualElement SetWidth(this VisualElement self, float v, LengthUnit unit = LengthUnit.Pixel)
        {
            StyleLength width = new Length(v, unit);
            self.style.width = width;

            return self;
        }

        public static VisualElement SetWidth(this VisualElement self, StyleKeyword keyword)
        {
            StyleLength width = new StyleLength(keyword);
            self.style.width = width;

            return self;
        }

        public static VisualElement ExpandWidth(this VisualElement self)
        {
            self.style.width = new Length(100, LengthUnit.Percent);

            return self;
        }

        public static VisualElement SetHeight(this VisualElement self, float v, LengthUnit unit = LengthUnit.Pixel)
        {
            StyleLength height = new Length(v, unit);
            self.style.height = height;

            return self;
        }

        public static VisualElement SetHeight(this VisualElement self, StyleKeyword keyword)
        {
            StyleLength height = new StyleLength(keyword);
            self.style.height = height;

            return self;
        }

        public static VisualElement ExpandHeight(this VisualElement self)
        {
            self.style.height = new Length(100, LengthUnit.Percent);

            return self;
        }

        public static VisualElement ExpandWidthAndHeight(this VisualElement self)
        {
            self.style.width = new Length(100, LengthUnit.Percent);
            self.style.height = new Length(100, LengthUnit.Percent);

            return self;
        }

        public static VisualElement SetMaxWidth(this VisualElement self, float v)
        {
            var c = self.style.maxWidth;
            c = v;
            self.style.maxWidth = c;

            return self;
        }

        public static VisualElement SetMaxHeight(this VisualElement self, float v)
        {
            var c = self.style.maxHeight;
            c = v;
            self.style.maxHeight = c;

            return self;
        }

        public static VisualElement SetMinWidth(this VisualElement self, float v)
        {
            var c = self.style.minWidth;
            c = v;
            self.style.minWidth = c;

            return self;
        }

        public static VisualElement SetMinHeight(this VisualElement self, float v)
        {
            var c = self.style.minHeight;
            c = v;
            self.style.minHeight = c;

            return self;
        }

        public static VisualElement SetFlexBasis(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.flexBasis;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.flexBasis = c;

            return self;
        }

        public static VisualElement SetFlexGrow(this VisualElement self, float v)
        {
            var c = self.style.flexGrow;
            c.value = v;
            self.style.flexGrow = c;

            return self;
        }

        public static VisualElement SetFlexShrink(this VisualElement self, float v)
        {
            var c = self.style.flexShrink;
            c.value = v;
            self.style.flexShrink = c;

            return self;
        }

        public static VisualElement SetFlexDirection(this VisualElement self, FlexDirection v)
        {
            var c = self.style.flexDirection;
            c.value = v;
            self.style.flexDirection = c;

            return self;
        }

        public static VisualElement SetColumn(this VisualElement self, bool reverse = false)
        {
            return SetFlexDirection(self, reverse ? FlexDirection.ColumnReverse : FlexDirection.Column);
        }

        public static VisualElement SetRow(this VisualElement self, bool reverse = false)
        {
            return SetFlexDirection(self, reverse ? FlexDirection.RowReverse : FlexDirection.Row);
        }

        public static VisualElement SetFlexWarp(this VisualElement self, Wrap v)
        {
            var c = self.style.flexWrap;
            c.value = v;
            self.style.flexWrap = c;

            return self;
        }

        public static VisualElement SetPositionLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.left;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.left = c;

            return self;
        }

        public static VisualElement SetPositionTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.top;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.top = c;

            return self;
        }

        public static VisualElement SetPositionRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.right;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.right = c;

            return self;
        }

        public static VisualElement SetPositionBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.bottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.bottom = c;

            return self;
        }

        public static VisualElement SetMargin(this VisualElement self, float v, LengthUnit v2 = LengthUnit.Pixel)
        {
            SetMarginBottom(self, v, v2);
            SetMarginLeft(self, v, v2);
            SetMarginRight(self, v, v2);
            SetMarginTop(self, v, v2);
            return self;
        }

        public static VisualElement SetMarginLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginLeft;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginLeft = c;

            return self;
        }

        public static VisualElement SetMarginTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginTop;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginTop = c;

            return self;
        }

        public static VisualElement SetMarginRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginRight;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginRight = c;

            return self;
        }

        public static VisualElement SetMarginBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.marginBottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.marginBottom = c;

            return self;
        }

        public static VisualElement SetPadding(this VisualElement self, float v, LengthUnit v2)
        {
            SetPaddingLeft(self, v, v2);
            SetPaddingRight(self, v, v2);
            SetPaddingBottom(self, v, v2);
            SetPaddingTop(self, v, v2);
            return self;
        }

        public static VisualElement SetPaddingLeft(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingLeft;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingLeft = c;

            return self;
        }

        public static VisualElement SetPaddingTop(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingTop;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingTop = c;

            return self;
        }

        public static VisualElement SetPaddingRight(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingRight;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingRight = c;

            return self;
        }

        public static VisualElement SetPaddingBottom(this VisualElement self, float v, LengthUnit v2)
        {
            var c = self.style.paddingBottom;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.paddingBottom = c;

            return self;
        }

        public static VisualElement SetPosition(this VisualElement self, Position v)
        {
            var c = self.style.position;
            c.value = v;
            self.style.position = c;

            return self;
        }

        public static VisualElement SetAlignSelf(this VisualElement self, Align v)
        {
            var c = self.style.alignSelf;
            c.value = v;
            self.style.alignSelf = c;

            return self;
        }

        public static VisualElement SetTextAlign(this VisualElement self, TextAnchor v)
        {
            var c = self.style.unityTextAlign;
            c.value = v;
            self.style.unityTextAlign = c;

            return self;
        }

        public static VisualElement SetFontStyle(this VisualElement self, FontStyle v)
        {
            var c = self.style.unityFontStyleAndWeight;
            c.value = v;
            self.style.unityFontStyleAndWeight = c;

            return self;
        }

        public static VisualElement SetFontSize(this VisualElement self, float v, LengthUnit v2 = LengthUnit.Pixel)
        {
            var c = self.style.fontSize;
            var c2 = c.value;
            c2.value = v;
            c2.unit = v2;
            c.value = c2;
            self.style.fontSize = c;

            return self;
        }

        public static VisualElement SetTextWrap(this VisualElement self, WhiteSpace v)
        {
            var c = self.style.whiteSpace;
            c.value = v;
            self.style.whiteSpace = c;

            return self;
        }

        public static VisualElement SetTextColor(this VisualElement self, Color v)
        {
            var c = self.style.color;
            c.value = v;
            self.style.color = c;

            return self;
        }

        public static VisualElement SetText(this TextElement self, string text)
        {
            self.text = text;

            return self;
        }

        public static VisualElement SetBackgroundColor(this VisualElement self, Color v)
        {
            var c = self.style.backgroundColor;
            c.value = v;
            self.style.backgroundColor = c;

            return self;
        }

        public static VisualElement SetBorderColor(this VisualElement self, Color v)
        {
            self.SetBorderLeftColor(v);
            self.SetBorderTopColor(v);
            self.SetBorderRightColor(v);
            self.SetBorderBottomColor(v);

            return self;
        }

        public static VisualElement SetBorderLeftColor(this VisualElement self, Color v)
        {
            var c = self.style.borderLeftColor;
            c.value = v;
            self.style.borderLeftColor = c;

            return self;
        }

        public static VisualElement SetBorderTopColor(this VisualElement self, Color v)
        {
            var c = self.style.borderTopColor;
            c.value = v;
            self.style.borderTopColor = c;

            return self;
        }

        public static VisualElement SetBorderRightColor(this VisualElement self, Color v)
        {
            var c = self.style.borderRightColor;
            c.value = v;
            self.style.borderRightColor = c;

            return self;
        }

        public static VisualElement SetBorderBottomColor(this VisualElement self, Color v)
        {
            var c = self.style.borderBottomColor;
            c.value = v;
            self.style.borderBottomColor = c;

            return self;
        }

        public static VisualElement SetFont(this VisualElement self, Font v)
        {
            var c = self.style.unityFont;
            c.value = v;
            self.style.unityFont = c;

            return self;
        }

        public static VisualElement SetBackgroundScaleMode(this VisualElement self, ScaleMode v)
        {
            var c = self.style.unityBackgroundScaleMode;
            c.value = v;
            self.style.unityBackgroundScaleMode = c;

            return self;
        }

        public static VisualElement SetBackgroundImageColor(this VisualElement self, Color v)
        {
            var c = self.style.unityBackgroundImageTintColor;
            c.value = v;
            self.style.unityBackgroundImageTintColor = c;

            return self;
        }

        public static VisualElement SetAlignItems(this VisualElement self, Align v)
        {
            var c = self.style.alignItems;
            c.value = v;
            self.style.alignItems = c;

            return self;
        }

        public static VisualElement SetAlignContent(this VisualElement self, Align v)
        {
            var c = self.style.alignContent;
            c.value = v;
            self.style.alignContent = c;

            return self;
        }

        public static VisualElement SetJustifyContent(this VisualElement self, Justify v)
        {
            var c = self.style.justifyContent;
            c.value = v;
            self.style.justifyContent = c;

            return self;
        }

        public static VisualElement SetBorderWidth(this VisualElement self, float v)
        {
            self.SetBorderLeftWidth(v);
            self.SetBorderTopWidth(v);
            self.SetBorderRightWidth(v);
            self.SetBorderBottomWidth(v);

            return self;
        }

        public static VisualElement SetBorderLeftWidth(this VisualElement self, float v)
        {
            var c = self.style.borderLeftWidth;
            c.value = v;
            self.style.borderLeftWidth = c;

            return self;
        }

        public static VisualElement SetBorderRightWidth(this VisualElement self, float v)
        {
            var c = self.style.borderRightWidth;
            c.value = v;
            self.style.borderRightWidth = c;

            return self;
        }

        public static VisualElement SetBorderTopWidth(this VisualElement self, float v)
        {
            var c = self.style.borderTopWidth;
            c.value = v;
            self.style.borderTopWidth = c;

            return self;
        }

        public static VisualElement SetBorderBottomWidth(this VisualElement self, float v)
        {
            var c = self.style.borderBottomWidth;
            c.value = v;
            self.style.borderBottomWidth = c;

            return self;
        }

        public static VisualElement SetBorderRadius(this VisualElement self, float v)
        {
            self.SetBorderTopLeftRadius(v);
            self.SetBorderTopRightRadius(v);
            self.SetBorderBottomLeftRadius(v);
            self.SetBorderBottomRightRadius(v);

            return self;
        }

        public static VisualElement SetBorderTopLeftRadius(this VisualElement self, float v)
        {
            var c = self.style.borderTopLeftRadius;
            c.value = v;
            self.style.borderTopLeftRadius = c;

            return self;
        }

        public static VisualElement SetBorderTopRightRadius(this VisualElement self, float v)
        {
            var c = self.style.borderTopRightRadius;
            c.value = v;
            self.style.borderTopRightRadius = c;

            return self;
        }

        public static VisualElement SetBorderBottomLeftRadius(this VisualElement self, float v)
        {
            var c = self.style.borderBottomLeftRadius;
            c.value = v;
            self.style.borderBottomLeftRadius = c;

            return self;
        }

        public static VisualElement SetBorderBottomRightRadius(this VisualElement self, float v)
        {
            var c = self.style.borderBottomRightRadius;
            c.value = v;
            self.style.borderBottomRightRadius = c;

            return self;
        }

        public static VisualElement SetSlide(this VisualElement self, int v)
        {
            self.SetSliceLeft(v);
            self.SetSliceRight(v);
            self.SetSliceTop(v);
            self.SetSliceBottom(v);

            return self;
        }

        public static VisualElement SetSliceLeft(this VisualElement self, int v)
        {
            var c = self.style.unitySliceLeft;
            c.value = v;
            self.style.unitySliceLeft = c;

            return self;
        }

        public static VisualElement SetSliceRight(this VisualElement self, int v)
        {
            var c = self.style.unitySliceRight;
            c.value = v;
            self.style.unitySliceRight = c;

            return self;
        }

        public static VisualElement SetSliceTop(this VisualElement self, int v)
        {
            var c = self.style.unitySliceTop;
            c.value = v;
            self.style.unitySliceTop = c;

            return self;
        }

        public static VisualElement SetSliceBottom(this VisualElement self, int v)
        {
            var c = self.style.unitySliceBottom;
            c.value = v;
            self.style.unitySliceBottom = c;

            return self;
        }

        public static VisualElement SetOpacity(this VisualElement self, float v)
        {
            var c = self.style.opacity;
            c.value = v;
            self.style.opacity = c;

            return self;
        }

        public static VisualElement SetVisibility(this VisualElement self, Visibility v)
        {
            var c = self.style.visibility;
            c.value = v;
            self.style.visibility = c;

            return self;
        }

        public static VisualElement SetDisplay(this VisualElement self, DisplayStyle v)
        {
            var c = self.style.display;
            c.value = v;
            self.style.display = c;

            return self;
        }

        public static VisualElement SetOverflow(this VisualElement self, Overflow v)
        {
            var c = self.style.overflow;
            c.value = v;
            self.style.overflow = c;

            return self;
        }

        public static VisualElement SetClicked(this VisualElement self, Action callback)
        {
            if (self is Button btn)
            {
                btn.clicked += callback;
            }
            else
            {
                self.AddManipulator(new Clickable(callback));
            }

            return self;
        }

        public static IBindable SetBindingPath(this IBindable self, string bindingPath)
        {
            self.bindingPath = bindingPath;

            return self;
        }

        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element,
            EventCallback<ChangeEvent<T>> onValueChange, T initialValue)
        {
            InitField(ve, element, onValueChange, () => initialValue);
        }

        // TODO: See if we can use `this INotifyValueChanged<T> ve` without `element` argument
        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element,
            EventCallback<ChangeEvent<T>> onValueChange, Func<T> getInitialValue)
        {
            element.RegisterValueChangedCallback(onValueChange);

            ve.RegisterCallback<AttachToPanelEvent>(InitValue);

            void InitValue(AttachToPanelEvent e)
            {
                element.value = getInitialValue();
                ve.UnregisterCallback<AttachToPanelEvent>(InitValue);
            }
        }

        public static void ToggleDisplayStyle(this VisualElement ve, bool value)
        {
            ve.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
