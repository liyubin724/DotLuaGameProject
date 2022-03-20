namespace DotEngine.FSM
{
    public class FSTransition : IFSTransition
    {
        public string From { get; set; }
        public string To { get; set; }
        public IFSCondition Condition { get; set; }

        protected FSMachine machine;
        public virtual void DoInitilize(FSMachine machine)
        {
            this.machine = machine;
        }

        public virtual void DoDestroy()
        {
        }
    }
}
