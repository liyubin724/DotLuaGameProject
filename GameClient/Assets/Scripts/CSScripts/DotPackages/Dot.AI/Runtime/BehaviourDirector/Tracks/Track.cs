using DotEngine.AI.BD.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BD.Tracks
{
    public enum NodeTrackCategory
    {
        None = 0,

        Actor,

        Max,
    }

    public enum TrackCategory
    {

    }

    public abstract class Track
    {
        public string Name = string.Empty;
        public List<Actions.Action> Actions = new List<Actions.Action>();
        
        public virtual void DoUpdate(float deltaTime)
        {

        }

        public virtual void DoStart()
        {

        }

        public virtual void DoPause()
        {

        }

        public virtual void DoResume()
        {

        }

        public virtual void DoStop()
        {

        }

    }
}
