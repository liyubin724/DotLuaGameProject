using DotEditor.GUIExt.DataGrid;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.IMGUI
{
    public class EasyListViewDrawer : ValueProviderLayoutDrawable<SystemObject>
    {
        private EasyListView listView = null;

        private bool isDataChanged = false;
        private string[] displayNames = new string[0];
        public string[] DisplayNames
        {
            get
            {
                return displayNames;
            }
            set
            {
                displayNames = value;
                isDataChanged = true;
            }
        }
        private SystemObject[] values = new SystemObject[0];
        public SystemObject[] Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                isDataChanged = true;
            }
        }

        protected override void OnLayoutDraw()
        {
            if (listView == null)
            {
                listView = new EasyListView();
                listView.HeaderContent = Text;
                if(Values.Length>0)
                {
                    listView.AddItems(DisplayNames, Values);
                }
                if(Value!=null)
                {
                    listView.SelectedItem = Value;
                }
                listView.OnSelectedChange = (data) =>
                {
                    Value = data;
                };

                isDataChanged = false;
            }
            if(isDataChanged)
            {
                listView.Clear();
                if (Values.Length > 0)
                {
                    listView.AddItems(DisplayNames, Values);
                }
            }

            listView.OnGUILayout();
        }
    }
}
