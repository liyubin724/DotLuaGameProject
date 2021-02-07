using DotEngine.Context;
using System;
using System.Collections.Generic;

namespace DotEngine.AI.FSM
{
    public delegate void StateChangedHandler(StateMachine machine, StateChangedEventArgs e);
    public delegate void CompletedHandler(StateMachine machine);

    public class StateMachine
    {
        public object Context { get; }
        public IContextContainer<Type> TypeContext { get => (IContextContainer < Type >) Context;}
        public IContextContainer<string> StrContext { get => (IContextContainer<string>)Context; }

        /// <summary>
        /// 当前正常执行的状态
        /// </summary>
        public StateBase CurrentState { get; private set; }
        public StateToken CurrentStateToken => CurrentState?.Token;

        public bool IsRunning => CurrentState != null;

        public event StateChangedHandler ChangedHandler;
        public event CompletedHandler CompletedHandler;

        private Dictionary<StateToken, StateBase> stateDic = new Dictionary<StateToken, StateBase>();

        public StateMachine():this(null)
        {
        }

        public StateMachine(object context)
        {
            Context = context;
        }

        /// <summary>
        /// 状态机中添加状态
        /// </summary>
        /// <param name="state"></param>
        public void RegisterState(StateBase state)
        {
            if(state == null)
            {
                throw new ArgumentNullException("StateMachine::RegisterState->state is null");
            }

            if(stateDic.ContainsKey(state.Token))
            {
                throw new InvalidOperationException($"StateMachine::RegisterState->{state.Token} already registered.");
            }
            
            stateDic.Add(state.Token, state);
            state.Initialize(this);
        }

        /// <summary>
        /// 设置初始状态
        /// </summary>
        /// <param name="initialState"></param>
        public void SetInitialState(StateToken initialState)
        {
            SetInitialState(initialState, null);
        }

        /// <summary>
        /// 设置初始状态
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="data"></param>
        public void SetInitialState(StateToken initialState,object data)
        {
            PerformTransitionTo(initialState, data);
        }

        /// <summary>
        /// 切换状态机到指定的状态
        /// </summary>
        /// <param name="stateToken"></param>
        public void PerformTransitionTo(StateToken stateToken)
        {
            PerformTransitionTo(stateToken, null);
        }

        /// <summary>
        /// 切换状态机的状态
        /// </summary>
        /// <param name="stateToken"></param>
        /// <param name="data"></param>
        public void PerformTransitionTo(StateToken stateToken, object data)
        {
            //如果目标状态与当前状态相同则忽略，保持当前状态
            if(CurrentStateToken == stateToken)
            {
                return;
            }

            //行为树结束
            if(stateToken == null)
            {
                if (IsRunning)
                {
                    CurrentState.OnExit(new StateExitEventArgs(null, data));
                    CurrentState = null;
                }
                OnComplete();
            }
            else
            {
                if(stateDic.TryGetValue(stateToken,out StateBase state))
                {
                    
                    if(!IsRunning)
                    {
                        CurrentState = state;
                        CurrentState.OnEnter(new StateEnterEventArgs(null, data));
                    }
                    else
                    {
                        if(CurrentStateToken != stateToken)
                        {
                            StateBase oldState = CurrentState;

                            CurrentState.OnExit(new StateExitEventArgs(stateToken, data));
                            CurrentState = state;
                            CurrentState.OnEnter(new StateEnterEventArgs(oldState.Token, data));

                            OnStateChanged(new StateChangedEventArgs(oldState, state));
                        }
                    }
                }else
                {
                    throw new InvalidOperationException($"StateMachine::PerformTransitionTo->state is not found.toke  = {stateToken}");
                }
            }
        }

        protected virtual void OnStateChanged(StateChangedEventArgs e)
        {
            ChangedHandler?.Invoke(this, e);
        }

        /// <summary>
        /// 状态机执行完成
        /// </summary>
        protected virtual void OnComplete()
        {
            CompletedHandler?.Invoke(this);
        }

        public virtual void DoUpdate(float deltaTime)
        {
            CurrentState?.DoUpdate(deltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void PerformAction(ActionToken action)
        {
            PreformAction(action, null);
        }

        public void PreformAction(ActionToken action,object data)
        {
            if(action == null)
            {
                throw new ArgumentNullException("StateMachine::PreformAction->actoin is null");
            }

            if(CurrentState == null)
            {
                throw new InvalidOperationException("StateMachine::PreformAction->CurrentState is null");
            }

            CurrentState.ExecuteAction(action, data);
        }
    }
}
