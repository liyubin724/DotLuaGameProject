namespace DotEngine.Core.Update
{
    public interface ILateUpdate
    {
        void DoLateUpdate(float deltaTime, float unscaleDeltaTime);
    }
}
