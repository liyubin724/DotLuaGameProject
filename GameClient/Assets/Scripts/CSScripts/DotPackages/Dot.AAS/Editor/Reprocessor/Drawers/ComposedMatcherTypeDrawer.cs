using DotEditor.AAS.Matchers;
using DotEditor.GUIExt.NativeDrawer;
using System.Collections.Generic;

namespace DotEditor.AAS.Reprocessor
{
    [CustomTypeDrawer(typeof(ComposedMatcher))]
    public class ComposedMatcherTypeDrawer : TypeDrawer
    {
        private ArrayDrawer matcherDrawer = null;

        public override void OnGUILayout()
        {
            if(matcherDrawer == null)
            {
                ComposedMatcher matcher = ItemDrawer.Value as ComposedMatcher;

                List<IAssetMatcher> matcheres = ItemDrawer.Value as List<IAssetMatcher>;
                matcherDrawer = new ArrayDrawer(matcher.matchers)
                {
                    Header = "Matchers",
                    IsShowScroll = false,
                    IsShowBox = true,
                    IsShowTargetType = true,
                    CreateNewItem = () =>
                    {
                        AssetMatcherUtility.ShowMenu((item) =>
                        {
                            matcher.matchers.Add(item);
                            matcherDrawer.Refresh();
                        });
                    }
                };
            }
            matcherDrawer.OnGUILayout();
        }
    }
}
