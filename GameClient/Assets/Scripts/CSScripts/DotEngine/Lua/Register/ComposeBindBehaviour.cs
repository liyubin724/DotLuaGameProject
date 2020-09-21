namespace DotEngine.Lua.Register
{
    public class ComposeBindBehaviour : ScriptBinderBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            base.OnInitFinished();
            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerObjectData.RegisterToLua(service.Env, Table);
                registerBehaviourData.RegisterToLua(service.Env, Table);
            }
        }
    }
}
