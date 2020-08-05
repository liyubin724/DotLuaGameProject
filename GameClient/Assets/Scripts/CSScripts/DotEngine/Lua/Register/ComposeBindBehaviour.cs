namespace DotEngine.Lua.Register
{
    public class ComposeBindBehaviour : ScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            base.OnInitFinished();
            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerObjectData.RegisterToLua(service.Env, ObjTable);
                registerBehaviourData.RegisterToLua(service.Env, ObjTable);
            }
        }
    }
}
