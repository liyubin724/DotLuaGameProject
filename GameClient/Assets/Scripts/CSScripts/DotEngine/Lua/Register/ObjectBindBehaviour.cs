namespace DotEngine.Lua.Register
{
    public class ObjectBindBehaviour : ScriptBinderBehaviour
    {
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if (service.IsValid())
            {
                registerObjectData.RegisterToLua(service.Env, Table);
            }
        }
    }
}
