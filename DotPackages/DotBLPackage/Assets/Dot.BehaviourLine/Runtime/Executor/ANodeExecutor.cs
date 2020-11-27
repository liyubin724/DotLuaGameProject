using DotEngine.BL.Node;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.BL.Executor
{
    public abstract class ANodeExecutor
    {
        public NodeData Data { get; private set; } = null;

        public T GetData<T>() where T: NodeData
        {
            return (T)Data;
        }

        public virtual void DoInit(NodeData nodeData)
        {
            Data = nodeData;
        }

        public virtual void DoReset()
        {

        }
    }
}

