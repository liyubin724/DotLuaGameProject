using DotEngine.NativeDrawer.Decorator;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    public class AssetAddressGroup : ScriptableObject
    {
        [Help("资源所属分组的名称")]
        public string groupName = "Asset Address Group";
        [Help("此配置是否生效")]
        public bool isEnable = true;

        [DotEngine.NativeDrawer.Decorator.Space]
        [Help("是否为主资源，只有标记为主资源的才能在脚本中通过地址进行加载")]
        public bool isMain = false;
        
        [DotEngine.NativeDrawer.Decorator.Space]
        [Help("资源是否需要预加载")]
        public bool isPreload = false;
        [Help("资源一旦加载后，是否永不删除")]
        public bool isNeverDestroy = false;

        [DotEngine.NativeDrawer.Decorator.Space]
        [Help("资源打包时的处理方式")]
        public AssetAddressOperation operation = new AssetAddressOperation();
        [DotEngine.NativeDrawer.Decorator.Space]
        [Help("根据指定的配置进行资源的筛选")]
        public List<AssetAddressFilter> filters = new List<AssetAddressFilter>();
    }
}
