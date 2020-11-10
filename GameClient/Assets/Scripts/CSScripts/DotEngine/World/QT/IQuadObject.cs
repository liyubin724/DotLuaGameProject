using System;

namespace DotEngine.World.QT
{
    public interface IQuadObject
    {
        int UniqueID { get; }
        AABB2D Bounds { get; set; }

        //QuadTree Tree { get; set; }

        bool IsBoundsChangeable { get; set; }
        event Action<IQuadObject,AABB2D,AABB2D> OnBoundsChanged;

        void OnEnterView();
        void OnExitView();
    }

    public interface IBoundsChangeable
    {

    }

    public interface IViewChangeable
    {
        void OnEnterView();
        void OnExitView();
    }
}
