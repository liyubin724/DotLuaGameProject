namespace DotEngine.FSM
{
    public interface IFSState
    {
        string Name { get; set; }
        bool IsDefault { get; set; }

        void DoInitilize(FSMachine machine);
        void DoEnter(string from);
        void DoUpdate(float deltaTime);
        void DoLeave(string to);
        void DoDestroy();
    }
}
