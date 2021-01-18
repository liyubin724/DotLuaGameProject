using DotEngine.Utilities;
using System;
using System.Collections;
using System.Reflection;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.EditorDrawer.Provider
{
    public class FieldValueProvider : IValueProvider
    {
        public SystemObject Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ArrayIndex { get; private set; } = -1;

        public bool IsArrayItem
        {
            get
            {
                return ArrayIndex >= 0;
            }
        }

        public string FieldName
        {
            get
            {
                if(IsArrayItem)
                {
                    return $"{Field.Name}[{ArrayIndex}]";
                }else
                {
                    return Field.Name;
                }
            }
        }

        public Type TargetType
        {
            get
            {
                return Target?.GetType();
            }
        }

        public Type ValueType
        {
            get
            {
                if(IsArrayItem)
                {
                    return TypeUtility.GetArrayOrListElementType(Field.FieldType);
                }
                else
                {
                    return Field.FieldType;
                }
            }
        }


        public SystemObject Value
        {
            get
            {
                SystemObject tValue = Field.GetValue(Target);
                if(tValue!=null)
                {
                    if(IsArrayItem)
                    {
                        return ((IList)tValue)[ArrayIndex];
                    }
                }
                return tValue;
            }
            set
            {
                if(IsArrayItem)
                {
                    SystemObject tValue = Field.GetValue(Target);
                }else
                {
                    Field.SetValue(Target, value);
                }

                OnValueChanged?.Invoke(value);
            }
        }


        public event Action<object> OnValueChanged;

        public FieldValueProvider(FieldInfo field,SystemObject target)
        {
            Field = field;
            Target = target;
        }


    }
}
