using DotEngine.GUIExt.NativeDrawer;
using System;
using System.Collections.Generic;

namespace DotEditor.AAS
{
    public enum AssetLabelAssignType
    {
        None = 0,
        Manual,
    }

    [Serializable]
    public class AssetLabelAssignRule
    {
        public AssetLabelAssignType assignType = AssetLabelAssignType.None;
        [VisibleIf("IsShowLabels")]
        public List<string> labels = new List<string>();

        public string[] GetLabels(string assetPath)
        {
            if(assignType == AssetLabelAssignType.None)
            {
                return new string[0];
            }else
            {
                return labels.ToArray();
            }
        }

        private bool IsShowLabels() => assignType == AssetLabelAssignType.Manual;
    }
}
