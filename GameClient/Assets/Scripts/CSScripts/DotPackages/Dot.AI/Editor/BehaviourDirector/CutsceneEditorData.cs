using DotEngine.BD.Datas;
using System;

namespace DotEditor.BD
{
    public class CutsceneEditorData
    {
        private CutsceneData m_SelectedCutscene = null;
        public event Action<CutsceneData> SelectedCutsceneChangedEvent;
        public CutsceneData Cutscene
        {
            get
            {
                return m_SelectedCutscene;
            }
            set
            {
                m_SelectedCutscene = value;
                SelectedCutsceneChangedEvent?.Invoke(m_SelectedCutscene);
            }
        }

        private GroupData m_SelectedGroup = null;
        public event Action<GroupData> SelectedGroupChangedEvent;
        public GroupData SelectedGroup
        {
            get
            {
                return m_SelectedGroup;
            }
            set
            {
                if (m_SelectedGroup != value)
                {
                    m_SelectedGroup = value;
                    SelectedGroupChangedEvent?.Invoke(m_SelectedGroup);
                }
            }
        }

        private TrackData m_SelectedTrack = null;
        public Action<TrackData> SelectedTrackChangedEvent;
        public TrackData SelectedTrackData
        {
            get
            {
                return m_SelectedTrack;
            }
            set
            {
                if (m_SelectedTrack != value)
                {
                    m_SelectedTrack = value;

                    if (m_SelectedTrack != null && m_SelectedGroup.Tracks.IndexOf(m_SelectedTrack) < 0)
                    {
                        foreach (var group in Cutscene.Groups)
                        {
                            if (group.Tracks.IndexOf(m_SelectedTrack) >= 0)
                            {
                                SelectedGroup = group;
                                break;
                            }
                        }
                    }
                    SelectedTrackChangedEvent.Invoke(m_SelectedTrack);
                }
            }
        }

        private ActionData m_SelectedAction = null;
        public Action<ActionData> SelectedActionChangedEvent;
        public ActionData SelectedAction
        {
            get
            {
                return m_SelectedAction;
            }
            set
            {
                if (m_SelectedAction != value)
                {
                    m_SelectedAction = value;
                    if (m_SelectedAction != null && SelectedTrackData.Actions.IndexOf(m_SelectedAction) < 0)
                    {
                        foreach (var group in Cutscene.Groups)
                        {
                            foreach (var track in group.Tracks)
                            {
                                if (track.Actions.IndexOf(m_SelectedAction) >= 0)
                                {
                                    SelectedGroup = group;
                                    SelectedTrackData = track;
                                    break;
                                }
                            }
                        }
                    }
                    SelectedActionChangedEvent?.Invoke(m_SelectedAction);
                }
            }
        }

        public ActionData CopiedAction { get; set; }
    }
}
