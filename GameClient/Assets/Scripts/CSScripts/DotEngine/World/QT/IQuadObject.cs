using System;

namespace DotEngine.World.QT
{
    /// <summary>
    /// 需要添加到四叉树中对象需要实现对应的接口
    /// </summary>
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
