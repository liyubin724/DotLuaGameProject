using System;

namespace DotEngine.World.QT
{
    public interface IQuadObject
    {
        int UniqueID { get; }
        AABB2D Bounds { get; set; }

        event Action<QuadObjectBoundChangeEventArgs> OnBoundsChanged;

        void OnEnterView();
        void OnExitView();
    }

    public class QuadObjectBoundChangeEventArgs : EventArgs
    {
        public IQuadObject Target { get; set; }
        public AABB2D OldBounds { get; set; }
        public AABB2D NewBounds { get; set; }

        public QuadObjectBoundChangeEventArgs(IQuadObject target,AABB2D oldBounds,AABB2D newBounds)
        {
            Target = target;
            OldBounds = oldBounds;
            NewBounds = newBounds;
        }
    }
}
