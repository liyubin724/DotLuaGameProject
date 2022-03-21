using System.Collections.Generic;
using System;

namespace DotEngine.FSM
{
    public class Machine
    {
        public Blackboard Blackboard { get; set; }
        public List<IState> States { get; set; }
        public List<ITransition> Transitions { get; set; }
        public string InitState { get; set; }
        public bool IsRunWhenInitlized { get; set; } = true;

        private Dictionary<string, IState> stateDic = new Dictionary<string, IState>();
        private Dictionary<string, List<ITransition>> transitionDic = new Dictionary<string, List<ITransition>>();

        private string currentState = null;
        public string CurrentState => currentState;

        private bool isRunning = false;
        public bool IsRunning => isRunning;

        public void DoInitilize()
        {
            if(Blackboard == null)
            {
                Blackboard = new Blackboard();
            }
            if(States!=null && States.Count>0)
            {
                foreach(var state in States)
                {
                    if(stateDic.ContainsKey(state.Name))
                    {
                        stateDic.Add(state.Name, state);
                        state.DoInitilize(this);
                    }
                }
            }
            if(Transitions!=null && Transitions.Count>0)
            {
                foreach(var transition in Transitions)
                {
                    if(!transitionDic.TryGetValue(transition.From,out var list))
                    {
                        list = new List<ITransition>();
                        transitionDic.Add(transition.From, list);
                    }
                    list.Add(transition);
                    transition.DoInitilize(this);
                }
            }

            if(IsRunWhenInitlized)
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
            if (!string.IsNullOrEmpty(currentState))
            {
                return;
            }
            if (string.IsNullOrEmpty(InitState))
            {
                return;
            }

            currentState = InitState;
            IState state = stateDic[currentState];
            state.DoEnter(null);
        }

        public void DoUpdate(float deltaTime)
        {
            if (!isRunning || string.IsNullOrEmpty(currentState))
            {
                return;
            }

            IState runningState = stateDic[currentState];
            runningState.DoUpdate(deltaTime);

            if (transitionDic.TryGetValue(currentState, out var transitions))
            {
                foreach (var transition in transitions)
                {
                    if (transition.Condition.IsSatisfy())
                    {
                        string nextStateName = transition.To;
                        runningState.DoLeave(nextStateName);

                        IState toState = stateDic[nextStateName];
                        toState.DoEnter(this.currentState);

                        this.currentState = nextStateName;
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
            currentState = null;
            Blackboard = null;
        }
    }
}
