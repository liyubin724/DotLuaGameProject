using System.Collections.Generic;
using System;

namespace DotEngine.FSM
{
    public delegate void StateChanged(string fromGuid, string toGuid);

    public class Machine
    {
        public string Guid { get; set; }
        public string DisplayName { get; set; }
        public string InitStateGuid { get; set; }
        public bool AutoRunWhenInitlized { get; set; } = true;

        public Blackboard Blackboard { get; set; }

        private string currentStateGuid = null;
        public string CurrentStateGuid => currentStateGuid;

        private bool isRunning = false;
        public bool IsRunning => isRunning;

        public event StateChanged OnStateChanged;

        private Dictionary<string, IState> stateDic = new Dictionary<string, IState>();
        private Dictionary<string, List<Transition>> transitionDic = new Dictionary<string, List<Transition>>();

        internal void AddState(IState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException();
            }
            if (stateDic.ContainsKey(state.Guid))
            {
                throw new ArgumentException();
            }

            stateDic.Add(state.Guid, state);
        }

        internal void AddTransition(Transition transition)
        {
            if (transition == null)
            {
                throw new ArgumentNullException();
            }
            if (!transitionDic.TryGetValue(transition.FromStateGuid, out var list))
            {
                list = new List<Transition>();
                transitionDic.Add(transition.FromStateGuid, list);
            }
            list.Add(transition);
        }

        public void DoInitilize()
        {
            if (Blackboard == null)
            {
                Blackboard = new Blackboard();
            }
            foreach (var state in stateDic.Values)
            {
                state.DoInitilize(this);
            }
            foreach (var list in transitionDic.Values)
            {
                foreach (var transition in list)
                {
                    transition.DoInitilize(this);
                }
            }

            if (AutoRunWhenInitlized)
            {
                DoActivate();
            }
        }

        public void DoActivate()
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            if (!string.IsNullOrEmpty(currentStateGuid))
            {
                return;
            }
            if (string.IsNullOrEmpty(InitStateGuid))
            {
                return;
            }

            TransitionTo(InitStateGuid);
        }

        public void DoUpdate(float deltaTime)
        {
            if (!isRunning || string.IsNullOrEmpty(currentStateGuid))
            {
                return;
            }

            IState runningState = stateDic[currentStateGuid];
            runningState.DoExecute(deltaTime);

            if (transitionDic.TryGetValue(currentStateGuid, out var transitions))
            {
                foreach (var transition in transitions)
                {
                    if (transition.Condition.IsSatisfy())
                    {
                        TransitionTo(transition.ToStateGuid);
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
            OnStateChanged = null;
            foreach (var state in stateDic.Values)
            {
                state.DoDestroy();
            }
            stateDic.Clear();
            foreach (var transitions in transitionDic.Values)
            {
                foreach (var transition in transitions)
                {
                    transition.DoDestroy();
                }
            }
            transitionDic.Clear();
            currentStateGuid = null;
            Blackboard.Clear();
            Blackboard = null;
        }

        private void TransitionTo(string toStateGuid)
        {
            if (toStateGuid == currentStateGuid)
            {
                return;
            }
            IState currentState = string.IsNullOrEmpty(currentStateGuid) ? null : stateDic[currentStateGuid];
            if (currentState != null)
            {
                currentState.DoLeave(toStateGuid);
            }

            string preStateGuid = currentStateGuid;

            currentStateGuid = toStateGuid;
            IState toState = stateDic[currentStateGuid];
            toState.DoEnter(preStateGuid);

            OnStateChanged?.Invoke(preStateGuid, currentStateGuid);
        }
    }
}
