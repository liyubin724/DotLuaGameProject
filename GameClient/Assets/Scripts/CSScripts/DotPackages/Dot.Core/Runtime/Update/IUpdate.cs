namespace DotEngine
{
    public interface IUpdater
    {
    }

    public interface IUpdate : IUpdater
    {
        void DoUpdate(float deltaTime);
    }

    public interface IUnscaleUpdate : IUpdater
    {
        void DoUnscaleUpdate(float unscaleDeltaTime);
    }

    public interface ILateUpdate : IUpdater
    {
        void DoLateUpdate(float deltaTime);
    }

    public interface IFixedUpdate : IUpdater
    {
        void DoFixedUpdate(float deltaTime);
    }
}
