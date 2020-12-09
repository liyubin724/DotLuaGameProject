using DotEngine.BD.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class BDWindow : EditorWindow
    {
        [MenuItem("Game/AI/Behaviour Director")]
        static void ShowWin()
        {
            var win = GetWindow<BDWindow>();
            win.titleContent = new GUIContent("Behaviour Director");
            win.Show();
        }

        private TrackGroupData trackGroupData;
        private TrackData trackData;
        private void OnGUI()
        {
            if(GUILayout.Button("Create TrackGroupData"))
            {
                MenuUtility.ShowCreateTrackGroupMenu((groupData) =>
                {
                    trackGroupData = groupData;
                });
            }
            if(GUILayout.Button("Create TrackData"))
            {
                MenuUtility.ShowCreateTrackMenu(trackGroupData.GetType(),(trackData)=>
                {
                    trackGroupData.Tracks.Add(trackData);
                });
            }

            if(GUILayout.Button("Create ActionData"))
            {
                MenuUtility.ShowCreateActionMenu(trackData.GetType(), (actionData) =>
                {
                    trackData.Actions.Add(actionData);
                });
            }
        }
    }
}
