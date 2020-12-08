using DotEngine.AI.BD.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BD
{
    public abstract class TrackGroup
    {
        public List<Track> Tracks = new List<Track>();

        protected CutsceneContext Context { get; private set; }
        public virtual void DoInit(CutsceneContext context)
        {
            Context = context;
        }

        public virtual void DoUpdate(float deltaTime)
        {
            foreach (var track in Tracks)
            {
                track.DoUpdate(deltaTime);
            }
        }

        public virtual void DoPlay()
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
