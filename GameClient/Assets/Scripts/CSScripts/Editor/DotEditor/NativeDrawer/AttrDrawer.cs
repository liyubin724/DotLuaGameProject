﻿using DotEngine.NativeDrawer;
using DotEngine.NativeDrawer.Condition;
using DotEngine.Utilities;
using System;

namespace DotEditor.NativeDrawer
{
    public abstract class AttrDrawer
    {
        public DrawerAttribute DrawerAttr { get; set; }
        public DrawerProperty DrawerProperty { get; set; }

        public T GetAttr<T>() where T:DrawerAttribute
        {
            return (T)DrawerAttr;
        }

        public abstract void OnGUILayout();
    }




    public abstract class CompareAttrDrawer : AttrDrawer
    {
        public object Target { get; private set; }
        protected CompareAttrDrawer(object target,CompareAttribute attr)
        {
            Target = target;
        }

        protected bool IsEqual()
        {
            CompareAttribute attr = GetAttr<CompareAttribute>();
            if (string.IsNullOrEmpty(attr.MemberName))
            {
                return true;
            }

            object comparedValue = DrawerUtility.GetMemberValue(attr.MemberName, Target);
            if (comparedValue == null && attr.Value == null)
            {
                return true;
            } if (comparedValue == null || attr.Value == null)
            {
                return false;
            }

            if (comparedValue.GetType() != attr.Value.GetType())
            {
                return false;
            }

            if (TypeUtility.IsCastableTo(comparedValue.GetType(), typeof(IComparable)))
            {
                int compared = ((IComparable)comparedValue).CompareTo((IComparable)attr.Value);

                if(compared == 0)
                {
                    if(attr.Symbol == CompareSymbol.Gte || attr.Symbol == CompareSymbol.Lte || attr.Symbol == CompareSymbol.Eq)
                    {
                        return true;
                    }else
                    {
                        return false;
                    }
                }else
                {
                    if(attr.Symbol == CompareSymbol.Neq)
                    {
                        return true;
                    }else if(attr.Symbol == CompareSymbol.Eq)
                    {
                        return false;
                    }

                    if (compared > 0 && (attr.Symbol == CompareSymbol.Gt || attr.Symbol == CompareSymbol.Gte))
                    {
                        return true;
                    }
                    else if (compared < 0 && (attr.Symbol == CompareSymbol.Lt || attr.Symbol == CompareSymbol.Lte))
                    {
                        return true;
                    }else
                    {
                        return false;
                    }
                }
            }
            else
            {
                bool result = comparedValue == attr.Value;
                if(result && (attr.Symbol == CompareSymbol.Eq || attr.Symbol == CompareSymbol.Lte || attr.Symbol == CompareSymbol.Gte))
                {
                    return true;
                }else if(!result && (attr.Symbol == CompareSymbol.Neq))
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }
    }
}