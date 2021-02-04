namespace DotEngine.Framework.Updater
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime, float unscaleDeltaTime);
    }
}