namespace DotEngine.Lua.Register
{
    public class ObjectBindBehaviour : ScriptBindBehaviour
    {
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerObjectData.RegisterToLua(service.Env, ObjTable);
            }
        }
    }
}
