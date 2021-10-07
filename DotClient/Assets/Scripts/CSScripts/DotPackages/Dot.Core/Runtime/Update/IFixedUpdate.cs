namespace DotEngine.Core.Update
{
    public interface IFixedUpdate
    {
        void DoFixedUpdate(float deltaTime, float unscaleDeltaTime);
    }
}
