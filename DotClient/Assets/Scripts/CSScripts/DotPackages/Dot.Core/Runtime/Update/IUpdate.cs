namespace DotEngine.Core.Update
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime, float unscaleDeltaTime);
    }
}
