using DotEditor.GUIExt.DataGrid;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.Layout
{
    public class EasyListViewDrawer : ValueProviderLayoutDrawable<SystemObject>
    {
        private EasyListView listView = null;

        public string[] DisplayNames { get; set; } = new string[0];
        public SystemObject[] Values { get; set; } = new SystemObject[0];

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
            }

            listView.OnGUILayout();
        }
    }
}
