using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.FSM
{
    public class FSMachine : IFSState
    {
        public string Name { get; set; }
        public FSBlackboard Blackboard { get; private set; }

        private Dictionary<string, IFSState> stateDic = null;

        public FSMachine()
        {
            Blackboard = new FSBlackboard();
            stateDic = new Dictionary<string, IFSState>();
        }

        public bool HasState(string name)
        {
            return stateDic.ContainsKey(name);
        }

        public bool AddState(IFSState state)
        {
            if (state == null)
            {
                return false;
            }
            if (stateDic.ContainsKey(state.Name))
            {
                return false;
            }
            stateDic.Add(state.Name, state);
            return true;
        }

        public void DoInitilize(FSMachine machine)
        {
            throw new NotImplementedException();
        }

        public void DoEnter(string from)
        {
            throw new NotImplementedException();
        }

        public void DoUpdate(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void DoLeave(string to)
        {
            throw new NotImplementedException();
        }

        public void DoDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
