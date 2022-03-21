namespace DotEngine.FSM
{
    public class Transition
    {
        public string Guid { get; set; }
        public string DisplayName { get; set; }

        public string FromStateGuid { get; set; }
        public string ToStateGuid { get; set; }
        public ICondition Condition { get; set; }

        protected Machine machine;
        public void DoInitilize(Machine machine)
        {
            this.machine = machine;
            Condition?.DoInitilize(machine.Blackboard);
        }

        public void DoDestroy()
        {
            Condition?.DoDestroy();
        }
    }
}
