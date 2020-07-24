namespace DotEngine.AI.FSM
{
    public abstract class StateEventArgs
    {
        public object UserData { get; } = null;

        protected StateEventArgs(object userData)
        {
            UserData = userData;
        }
    }

    public class StateEnterEventArgs : StateEventArgs
    {
        public StateToken From { get; }
        
        public StateEnterEventArgs(StateToken from):this(from,null)
        {
        }

        public StateEnterEventArgs(StateToken from,object data) : base(data)
        {
            From = from;
        }
    }

    /// <summary>
    /// 状态退出时携带的参数
    /// </summary>
    public class StateExitEventArgs : StateEventArgs
    {
        /// <summary>
        /// 下一状态
        /// </summary>
        public StateToken To { get; }

        public StateExitEventArgs(StateToken to) : this(to,null)
        {
        }

        public StateExitEventArgs(StateToken to,object data) : base(data)
        {
            To = to;
        }
    }

    /// <summary>
    /// 状态机状态切换时带的数据
    /// </summary>
    public class StateChangedEventArgs : StateEventArgs
    {
        /// <summary>
        /// 上一个状态
        /// </summary>
        public StateBase OldState { get; }
        /// <summary>
        /// 下一状态
        /// </summary>
        public StateBase NewState { get; }

        public StateChangedEventArgs(StateBase oldState,StateBase newState) : base(null)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}
