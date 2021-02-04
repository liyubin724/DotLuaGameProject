namespace DotEngine.Framework.Updater
{
    public interface ILateUpdate
    {
        void DoLateUpdate(float deltaTime, float unscaleDeltaTime);
    }
}