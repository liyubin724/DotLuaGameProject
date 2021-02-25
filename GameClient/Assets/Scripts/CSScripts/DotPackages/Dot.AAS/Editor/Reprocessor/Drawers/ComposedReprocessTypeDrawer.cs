using DotEditor.GUIExt.NativeDrawer;

namespace DotEditor.AAS.Reprocessor
{
    [CustomTypeDrawer(typeof(ComposedReprocess))]
    public class ComposedReprocessTypeDrawer : TypeDrawer
    {
        private ArrayDrawer reprocessDrawer = null;

        public override void OnGUILayout()
        {
            if(reprocessDrawer == null)
            {
                ComposedReprocess reprocess = ItemDrawer.Value as ComposedReprocess;
                reprocessDrawer = new ArrayDrawer(reprocess.reprocesses)
                {
                    Header = "Reprocesses",
                    IsShowScroll = false,
                    IsShowBox = true,
                    IsShowTargetType = true,
                    CreateNewItem = () =>
                    {
                        AssetReprocessUtility.ShowMenu((item) =>
                        {
                            reprocess.reprocesses.Add(item);
                            reprocessDrawer.Refresh();
                        });
                    }
                };
            }
            reprocessDrawer.OnGUILayout();
        }
    }
}
