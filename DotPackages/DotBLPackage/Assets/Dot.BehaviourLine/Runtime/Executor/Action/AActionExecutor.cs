using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.BL.Executor.Action
{
    public abstract class AActionExecutor : ANodeExecutor
    {
        public float TimeScale { get; set; } = 1.0f;

        public virtual void DoInit()
        {

        }

        public abstract void DoExecute();

        public virtual void DoReset()
        {

        }
    }
}
