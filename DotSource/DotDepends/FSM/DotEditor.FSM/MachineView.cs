using DotEngine.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DotEditor.FSM
{
    public class MachineView : VisualElement
    {
        public class Factory : UxmlFactory<MachineView, Traits> { }
        public class Traits : UxmlTraits { }

        private MachineData bindedData = null;
        public MachineData BindedData
        {
            get
            {
                return bindedData;
            }
            set
            {
                if(bindedData!=value)
                {
                    bindedData = value;
                }
            }
        }
    }
}
