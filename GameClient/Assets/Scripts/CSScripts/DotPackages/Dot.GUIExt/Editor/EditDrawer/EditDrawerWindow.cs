using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DotEditor.GUIExt.EditDrawer
{
    public class EditDrawerWindow : EditorWindow
    {
        [MenuItem("Test/Edit Drawer Win")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<EditDrawerWindow>();
            win.Show();
        }

        ClickableSpringLabel csLabel;
        HorizontalToolbar hToolbar;
        private void OnEnable()
        {
            csLabel = new ClickableSpringLabel()
            {
                Text = "Clickable Spring Label",
                Tooltip = "Tooltip for CSL",
                OnClicked = () =>
                {
                    EditorUtility.DisplayDialog("Info", "CSL was clicked", "OK");
                },
                Style = EditorStyles.toolbarTextField,
            };

            hToolbar = new HorizontalToolbar()
            {
                LeftDrawable = new HorizontalCompositeDrawable(csLabel, csLabel, csLabel)
            };

        }


        private void OnGUI()
        {
            hToolbar.OnGUILayout();   
        }
    }
}
