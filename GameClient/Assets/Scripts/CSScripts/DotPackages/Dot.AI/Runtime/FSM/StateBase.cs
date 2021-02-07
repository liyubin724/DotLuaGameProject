using DotEngine.Context;
using System;
using System.Collections.Generic;

namespace DotEngine.AI.FSM
{
    public delegate void StateInitializedHandler(StateBase state);
    public delegate void StateEnterHandler(StateBase state,StateEnterEventArgs e);
    public delegate void StateExitHandler(StateBase state,StateExitEventArgs e);

    public delegate void StateActionHandler(ActionToken action, object data);

    public abstract class StateBase
    {
        /// <summary>
        /// 当前状态的标识
        /// </summary>
        public StateToken Token { get; }

        /// <summary>
        /// 此状态所属的状态机
        /// </summary>
        public StateMachine Machine { get; private set; }

        public object Context
        {
            get
            {
                if (Machine != null)
                {
                    return Machine.Context;
                }
                return null;
            }
        }

        public IContextContainer<Type> TypeContext
        {
            get
            {
                if(Machine!=null)
                {
                    return Machine.TypeContext;
                }
                return null;
            }
        }

        public IContextContainer<string> StrContext
        {
            get
            {
                if (Machine != null)
                {
                    return Machine.StrContext;
                }
                return null;
            }
        }

        public event StateInitializedHandler InitializedHandler;
        public event StateEnterHandler EnterHandler;
        public event StateExitHandler ExitHandler;

        private Dictionary<ActionToken, StateActionHandler> actionHandlerDic = new Dictionary<ActionToken, StateActionHandler>();

        protected StateBase(StateToken token)
        {
            if(token == null)
            {
                throw new ArgumentNullException("StateBase::StateBase->token is null");
            }

            Token = token;
        }

        /// <summary>
        /// 当状态首次被添加到状态机时会被调用。不要手动调用此函数
        /// </summary>
        /// <param name="machine"></param>
        internal void Initialize(StateMachine machine)
        {
            Machine = machine;

            OnInitialized();
        }

        /// <summary>
        /// 当State首次添加到Machine后会被调用到
        /// 子类State可以通过重写从面进行部分内容的初始化
        /// </summary>
        protected virtual void OnInitialized()
        {
            InitializedHandler?.Invoke(this);
        }

        /// <summary>
        /// 进入此State时会被调用
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEnter(StateEnterEventArgs e)
        {
            EnterHandler?.Invoke(this, e);
        }

        /// <summary>
        /// 退出此State时会被调用
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnExit(StateExitEventArgs e)
        {
            ExitHandler?.Invoke(this, e);
        }

        protected void ChangeState(StateToken stateToken, object data)
        {
            if (stateToken != Token)
            {
                Machine?.PerformTransitionTo(stateToken, data);
            }
        }

        public virtual void DoUpdate(float deltaTime)
        {
        }

        protected void RegisterAction(ActionToken action,StateActionHandler actionHandler)
        {
            if(action == null)
            {
                throw new ArgumentNullException("StateBase::RegisterAction->action is null");
            }

            if(actionHandler == null)
            {
                throw new ArgumentNullException("StateBase::RegisterAction->actionHandler is null");
            }

            if(actionHandlerDic.ContainsKey(action))
            {
                throw new InvalidOperationException($"Action has been registered.action = {action}.");
            }
            actionHandlerDic.Add(action, actionHandler);
        }

        protected void UnregisterAction(ActionToken action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("StateBase::UnregisterAction->action is null");
            }
            if(actionHandlerDic.ContainsKey(action))
            {
                actionHandlerDic.Remove(action);
            }
        }

        internal void ExecuteAction(ActionToken action,object data)
        {
            if(actionHandlerDic.TryGetValue(action,out StateActionHandler handler))
            {
                handler(action, data);
            }
        }

        public override string ToString()
        {
            return Token?.ToString() ?? "(Null Token)";
        }
    }
}
