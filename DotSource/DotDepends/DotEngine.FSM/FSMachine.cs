using System.Collections.Generic;

namespace DotEngine.FSM
{
    public class FSMachine
    {
        public FSBlackboard Blackboard { get; private set; }

        private Dictionary<string, IFSState> stateDic = new Dictionary<string, IFSState>();
        private Dictionary<string, List<IFSTransition>> transitionDic = new Dictionary<string, List<IFSTransition>>();

        private string defaultStateName = null;
        private string currentStateName = null;
        private bool isRunning = false;

        public bool IsRunning => isRunning;

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
            state.DoInitilize(this);

            if (state.IsDefault)
            {
                defaultStateName = state.Name;
            }
            return true;
        }

        public void AddTransition(FSTransition transition)
        {
            if (transition == null)
            {
                return;
            }

            if (!transitionDic.TryGetValue(transition.From, out var list))
            {
                list = new List<IFSTransition>();
                transitionDic.Add(transition.From, list);
            }
            list.Add(transition);
            transition.DoInitilize(this);
        }

        public void DoInitilize(FSBlackboard blackboard = null)
        {
            Blackboard = blackboard ?? new FSBlackboard();
        }

        public void DoActivate()
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            if (!string.IsNullOrEmpty(currentStateName))
            {
                return;
            }
            if (string.IsNullOrEmpty(defaultStateName))
            {
                return;
            }

            currentStateName = defaultStateName;
            IFSState state = stateDic[currentStateName];
            state.DoEnter(null);
        }

        public void DoUpdate(float deltaTime)
        {
            if (!isRunning || string.IsNullOrEmpty(currentStateName))
            {
                return;
            }

            IFSState currentState = stateDic[currentStateName];
            currentState.DoUpdate(deltaTime);

            if (transitionDic.TryGetValue(currentStateName, out var transitions))
            {
                foreach (var transition in transitions)
                {
                    if (transition.Condition.IsSatisfy())
                    {
                        string nextStateName = transition.To;
                        currentState.DoLeave(nextStateName);

                        IFSState toState = stateDic[nextStateName];
                        toState.DoEnter(currentStateName);

                        currentStateName = nextStateName;
                        break;
                    }
                }
            }
        }

        public void DoDeactivate()
        {
            isRunning = false;
        }

        public void DoDestroy()
        {
            isRunning = false;
            foreach(var state in stateDic.Values)
            {
                state.DoDestroy();
            }
            stateDic.Clear();
            foreach(var transitions in transitionDic.Values)
            {
                foreach(var transition in transitions)
                {
                    transition.DoDestroy();
                }
            }
            transitionDic.Clear();
            currentStateName = null;
            defaultStateName = null;
            Blackboard = null;
        }
    }
}
