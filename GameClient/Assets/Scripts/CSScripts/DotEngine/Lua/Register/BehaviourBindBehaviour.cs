namespace DotEngine.Lua.Register
{
    public class BehaviourBindBehaviour : ScriptBinderBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();

        protected override void OnInitFinished()
        {
            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerBehaviourData.RegisterToLua(service.Env, Table);
            }
        }
    }
}
