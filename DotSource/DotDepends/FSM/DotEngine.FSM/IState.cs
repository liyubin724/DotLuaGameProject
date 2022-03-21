namespace DotEngine.FSM
{
    public interface IState
    {
        string Guid { get; set; }
        string DisplayName { get; set; }

        void DoInitilize(Machine machine);
        void DoEnter(string from);
        void DoExecute(float deltaTime);
        void DoLeave(string to);
        void DoDestroy();
    }
}
