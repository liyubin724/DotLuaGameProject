using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BL.Executor.Action
{
    public abstract class ADurationActionExecutor : AActionExecutor
    {
        public abstract void DoEnter();
        public abstract void DoUpdate(float deltaTime,float unscaleDeltaTime);
        public abstract void DoExit();

        public virtual void DoPause() { }
        public virtual void DoResume() { }
    }
}
