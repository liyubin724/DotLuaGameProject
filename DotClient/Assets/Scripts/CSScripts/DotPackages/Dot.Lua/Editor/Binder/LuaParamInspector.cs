using DotEngine.Lua;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Lua
{
    public class LuaParamInspector : Inspector<LuaParamValue>
    {
        public override VisualElement Build()
        {
            VisualElement element = new VisualElement();

            EnumField field = new EnumField("ParamType", Target.paramType);
            element.Add(field);
            EnumField field2 = new EnumField("ParamType", Target.paramType);
            element.Add(field2);

            return element;
        }
    }

}

