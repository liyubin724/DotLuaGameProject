using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace DotEditor.AI.BT
{
    public enum BTELinkDirection
    {
        Parent = 0,
        Child,
    }

    public class BTLinkPortView : Port
    {


        protected BTLinkPortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
        }
    }
}
