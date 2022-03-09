namespace DotEngine.Updater
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime, float unscaleDeltaTime);
    }

    public interface ILateUpdate
    {
        void DoLateUpdate(float deltaTime, float unscaleDeltaTime);
    }

    public interface IFixedUpdate
    {
        void DoFixedUpdate(float deltaTime, float unscaleDeltaTime);
    }
}
