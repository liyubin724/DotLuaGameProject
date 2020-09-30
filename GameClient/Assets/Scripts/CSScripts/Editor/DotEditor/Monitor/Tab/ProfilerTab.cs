using DotEngine.Monitor.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.Monitor
{
    public enum ProfilerTabType
    {
        FPS,
        Log,
        Device,
        Memory,
    }

    public abstract class ProfilerTab
    {
        public ProfilerTabType TabType { get; private set; }

        public ProfilerTab(ProfilerTabType type)
        {
            TabType = type;
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }


        public void OnGUI(Rect rect)
        {

        }
    }
}
