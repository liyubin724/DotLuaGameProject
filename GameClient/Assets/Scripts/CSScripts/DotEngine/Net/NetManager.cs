namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        public void DoUpdate(float deltaTime)
        {
            DoUpdate_Client(deltaTime);
            DoUpdate_Server(deltaTime);
        }

        public void DoLateUpdate(float deltaTime)
        {
            DoLateUpdate_Client(deltaTime);
            DoLateUpdate_Server(deltaTime);
        }

        public override void DoDispose()
        {
            DoDispose_Client();
            DoDispose_Server();

            base.DoDispose();
        }
    }
}
