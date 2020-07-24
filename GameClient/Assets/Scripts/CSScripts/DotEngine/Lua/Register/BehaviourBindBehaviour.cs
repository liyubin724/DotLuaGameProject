namespace DotEngine.Lua.Register
{
    public class BehaviourBindBehaviour : ScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();

        protected override void OnInitFinished()
        {
            registerBehaviourData.RegisterToLua(Env, ObjTable);
        }
    }
}
