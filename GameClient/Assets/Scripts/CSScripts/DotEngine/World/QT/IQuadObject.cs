using UnityEngine;

namespace DotEngine.World.QT
{
    public delegate void BoundsChanged(IQuadObject quadObject);

    public interface IQuadObject
    {
        Rect ProjectRect { get; }
        Bounds Bounds { get; }
        event BoundsChanged BoundsChangedHandler;
    }
}
