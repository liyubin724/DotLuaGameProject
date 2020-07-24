namespace DotEngine.Lua.Register
{
    public class BehaviourBindBehaviour : ScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();

        protected override void OnInitFinished()
        {
            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerBehaviourData.RegisterToLua(service.Env, ObjTable);
            }
        }
    }
}
