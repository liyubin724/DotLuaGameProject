namespace DotEngine
{
    public interface ICloneable
    {
        object Clone();
        void CloneTo(object target);
    }
}
