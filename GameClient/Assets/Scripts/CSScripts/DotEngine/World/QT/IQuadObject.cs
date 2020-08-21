using UnityEngine;

namespace DotEngine.World.QT
{
    public delegate void BoundsChanged(IQuadObject quadObject);

    public interface IQuadObject
    {
        Rect Bounds { get; }
        event BoundsChanged BoundsChangedHandler;
    }
}
