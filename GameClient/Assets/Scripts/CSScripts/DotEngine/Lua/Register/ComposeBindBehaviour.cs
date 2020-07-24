namespace DotEngine.Lua.Register
{
    public class ComposeBindBehaviour : ScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            registerObjectData.RegisterToLua(Env, ObjTable);
            registerBehaviourData.RegisterToLua(Env, ObjTable);
        }
    }
}
