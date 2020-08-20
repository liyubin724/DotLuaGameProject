using System;
using UnityEngine;

namespace DotEngine.World.QT
{
    public delegate void QuadObjectBoundsChanged(IQuadObject quadObject);

    public interface IQuadObject
    {
        Rect Bounds { get; }
        event QuadObjectBoundsChanged BoundsChanged;

        void DoShow();
        void DoHidden();
        void DoLOD(int level);
    }
}
