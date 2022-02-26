using DotEngine.AI.BT.Attributes;
using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    internal static class BTNodeFactory
    {
        private static Dictionary<Type, Type> dataToNodeDic = new Dictionary<Type, Type>();
        static BTNodeFactory()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.IsAbstract || type.IsInterface || type.IsValueType || type.IsInterface || type.IsSealed)
                    {
                        continue;
                    }
                    var attrs = type.GetCustomAttributes(typeof(BTNodeLinkedDataAttribute),false);
                    if(attrs!=null && attrs.Length>0)
                    {
                        var attr = attrs[0] as BTNodeLinkedDataAttribute;
                        if(!dataToNodeDic.ContainsKey(type))
                        {
                            dataToNodeDic.Add(attr.DataType, type);
                        }
                    }
                }
            }
        }

        public static BTANode CreateNode(BTController controller,BTNodeData data)
        {
            if(controller == null || data == null)
            {
                throw new NullReferenceException();
            }
            var dataType = data.GetType();
            if(dataToNodeDic.TryGetValue(dataType ,out var nodeType))
            {
                if (!(Activator.CreateInstance(nodeType) is BTANode node))
                {
                    throw new NullReferenceException();
                }
                node.DoInitilize(controller,data);
                return node;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public static BTAExecutorNode CreateExecutorNode(BTController controller,BTExecutorNodeData executorData)
        {
            return CreateNode(controller, executorData) as BTAExecutorNode;
        }

        public static BTAConditionNode CreateConditionNode(BTController controller, BTConditionNodeData conditionData)
        {
            return CreateNode(controller, conditionData) as BTAConditionNode;
        }
    }
}
