using DotEngine.FSM;
using DotEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.FSM
{
    public class BlackboardView : VisualElement
    {
        public class Factory : UxmlFactory<BlackboardView, Traits>
        {

        }

        public class Traits : UxmlTraits
        {
            UxmlStringAttributeDescription m_Key = new UxmlStringAttributeDescription() { name = "key" };
            UxmlEnumAttributeDescription<BlackboardValueType> m_ValueType = new UxmlEnumAttributeDescription<BlackboardValueType>() { name = "type" };
            UxmlBoolAttributeDescription m_BoolValue = new UxmlBoolAttributeDescription() { name = "boolValue" };
            UxmlStringAttributeDescription m_StringValue = new UxmlStringAttributeDescription() { name = "stringValue" };
            UxmlIntAttributeDescription m_IntValue = new UxmlIntAttributeDescription() { name = "intValue" };
            UxmlFloatAttributeDescription m_floatValue = new UxmlFloatAttributeDescription() { name = "floatValue" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                BlackboardView view = ve as BlackboardView;
                if (view.BindedData != null)
                {
                    view.BindedData.Key = m_Key.GetValueFromBag(bag, cc);
                    view.BindedData.ValueType = m_ValueType.GetValueFromBag(bag, cc);
                    if (view.BindedData.ValueType == BlackboardValueType.Bool)
                    {
                        view.BindedData.BoolValue = m_BoolValue.GetValueFromBag(bag, cc);
                    }
                    else if (view.BindedData.ValueType == BlackboardValueType.String)
                    {
                        view.BindedData.StrValue = m_StringValue.GetValueFromBag(bag, cc);
                    }
                    else if (view.BindedData.ValueType == BlackboardValueType.Int)
                    {
                        view.BindedData.IntValue = m_IntValue.GetValueFromBag(bag, cc);
                    }
                    else if (view.BindedData.ValueType == BlackboardValueType.Float)
                    {
                        view.BindedData.FloatValue = m_IntValue.GetValueFromBag(bag, cc);
                    }
                    view.UpdateFieldVisible();
                }
            }
        }
        private BlackboardData bindedData = null;
        public BlackboardData BindedData
        {
            get
            {
                return bindedData;
            }
            set
            {
                if (bindedData != value)
                {
                    bindedData = value;
                    UpdateFieldVisible();
                }
            }
        }

        [Q("key")]
        private TextField m_KeyField;
        [Q("value")]
        private VisualElement m_ValueContainer;
        [Q("value-type")]
        private EnumField m_ValueTypeField;
        [Q("bool-value")]
        private Toggle m_BoolValueField;
        [Q("string-value")]
        private TextField m_StringValueField;
        [Q("int-value")]
        private IntegerField m_IntValueField;
        [Q("float-value")]
        private FloatField m_FloatValueField;

        private VisualElement m_ShowingElement = null;

        public BlackboardView()
        {
            var visualTreeAsset = Resources.Load<VisualTreeAsset>("dot_fsm_blackboard_uxml");
            var visualTree = visualTreeAsset.CloneTree();
            Add(visualTree);

            visualTree.AssignQueryResults(this);
            m_ValueContainer.Remove(m_BoolValueField);
            m_ValueContainer.Remove(m_StringValueField);
            m_ValueContainer.Remove(m_IntValueField);
            m_ValueContainer.Remove(m_FloatValueField);

            UpdateFieldVisible();
            m_ValueTypeField.RegisterValueChangedCallback((e) =>
            {
                BindedData.ValueType = (BlackboardValueType)e.newValue;
                UpdateFieldVisible();
            });
            m_BoolValueField.RegisterValueChangedCallback((e) =>
            {
                BindedData.BoolValue = m_BoolValueField.value;
                UpdateFieldVisible();
            });
            m_StringValueField.RegisterValueChangedCallback((e) =>
            {
                BindedData.StrValue = m_StringValueField.value;
                UpdateFieldVisible();
            });
            m_IntValueField.RegisterValueChangedCallback((e) =>
            {
                BindedData.IntValue = m_IntValueField.value;
                UpdateFieldVisible();
            });
            m_FloatValueField.RegisterValueChangedCallback((e) =>
            {
                BindedData.FloatValue = m_FloatValueField.value;
                UpdateFieldVisible();
            });
        }

        private void UpdateFieldVisible()
        {
            if (m_ShowingElement != null)
            {
                m_ValueContainer.Remove(m_ShowingElement);
            }

            if (BindedData != null)
            {
                m_ValueTypeField.SetValueWithoutNotify(bindedData.ValueType);
                if (BindedData.ValueType == BlackboardValueType.Bool)
                {
                    m_ShowingElement = m_BoolValueField;
                }
                else if (BindedData.ValueType == BlackboardValueType.String)
                {
                    m_ShowingElement = m_StringValueField;
                }
                else if (BindedData.ValueType == BlackboardValueType.Int)
                {
                    m_ShowingElement = m_IntValueField;
                }
                else if (BindedData.ValueType == BlackboardValueType.Float)
                {
                    m_ShowingElement = m_FloatValueField;
                }
                if (m_ShowingElement != null)
                {
                    m_ValueContainer.Add(m_ShowingElement);
                }
            }
        }
    }
}
