namespace DotEngine.FSM
{
    public class Transition : ITransition
    {
        public string From { get; set; }
        public string To { get; set; }
        public ICondition Condition { get; set; }

        protected Machine machine;
        public virtual void DoInitilize(Machine machine)
        {
            this.machine = machine;
        }

        public virtual void DoDestroy()
        {
        }
    }
}
