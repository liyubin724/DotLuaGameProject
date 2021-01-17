namespace DotEngine.BD.Executors
{
    public abstract class EventActionExecutor : ActionExecutor
    {
        protected EventActionExecutor():base()
        {

        }

        public abstract void DoTrigger();
    }
}
