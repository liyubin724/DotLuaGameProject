using DotEngine.World.QT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.World
{
    public enum WorldQTEntityState
    {
        None = 0,
    }

    public interface IWorldQTEntity : IQuadObject
    {
        WorldQTEntityState State { get; }

        void DoShow(int lod);
        void DoHidden();
    }
}
