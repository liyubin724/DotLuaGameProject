using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT
{
    public class BTController
    {
        private BTContext context = new BTContext();
        public BTContext Context
        {
            get
            {
                return context;
            }
        }

        private bool isRunning = false;
        public bool IsRunning => isRunning;

        public void DoInitilize()
        {

        }

        public void DoActivate()
        {
            
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {

        }

        public void DoDeactivate()
        {

        }

        public void DoDestroy()
        {

        }
    }
}
