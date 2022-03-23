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


        public BlackboardView()
        {
            var visualTreeAsset = Resources.Load<VisualTreeAsset>("dot_fsm_blackboard_uxml");
            var visualTree = visualTreeAsset.CloneTree();
            Add(visualTree);

            visualTree.AssignQueryResults(this);
            //this.SetDisplay(DisplayStyle.Flex);
            //this.SetRow();

            //m_KeyField = new TextField("Key");

            //Add(m_KeyField);

            //m_ValueTypeField = new EnumField("ValueType", BlackboardValueType.String);
            //m_ValueTypeField.RegisterValueChangedCallback((e) =>
            //{
            //    BindedData.ValueType = (BlackboardValueType)e.newValue;
            //    UpdateFieldVisible();
            //});
            //Add(m_ValueTypeField);

            //m_BoolValueField = new Toggle("BoolValue");
            //Add(m_BoolValueField);

            //m_StringValueField = new TextField("StringValue");
            //Add(m_StringValueField);

            //m_IntValueField = new IntegerField("IntValue");
            //Add(m_IntValueField);

            //m_FloatValueField = new FloatField("FloatValue");
            //Add(m_FloatValueField);

            //UpdateFieldVisible();
        }

        private void UpdateFieldVisible()
        {
            m_ValueTypeField.SetVisibility(Visibility.Hidden);
            m_BoolValueField.SetVisibility(Visibility.Hidden);
            m_StringValueField.SetVisibility(Visibility.Hidden);
            m_IntValueField.SetVisibility(Visibility.Hidden);
            m_FloatValueField.SetVisibility(Visibility.Hidden);
            if (BindedData != null)
            {
                m_ValueTypeField.SetVisibility(Visibility.Visible);
                if (BindedData.ValueType == BlackboardValueType.Bool)
                {
                    m_BoolValueField.SetVisibility(Visibility.Visible);
                }
                else if (BindedData.ValueType == BlackboardValueType.String)
                {
                    m_StringValueField.SetVisibility(Visibility.Visible);
                }
                else if (BindedData.ValueType == BlackboardValueType.Int)
                {
                    m_IntValueField.SetVisibility(Visibility.Visible);
                }
                else if (BindedData.ValueType == BlackboardValueType.Float)
                {
                    m_FloatValueField.SetVisibility(Visibility.Visible);
                }
            }
            
        }
    }
}
