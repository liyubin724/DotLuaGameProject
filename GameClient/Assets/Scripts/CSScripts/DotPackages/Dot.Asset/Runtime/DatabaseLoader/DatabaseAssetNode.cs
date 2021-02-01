#if UNITY_EDITOR
using DotEngine.Asset.Datas;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    internal class DatabaseAssetNode : AAssetNode
    {
        private UnityObject uObject = null;
        private bool isDone = false;
        internal void SetAsset(UnityObject uObj)
        {
            uObject = uObj;
            isDone = true;
        }

        protected internal override UnityObject GetAsset()
        {
            if(uObject != null)
            {
                return uObject;
            }else
            {
                DebugLog.Error(AssetConst.LOGGER_NAME, "Asset is null");
                return null;
            }
        }

        protected internal override UnityObject GetInstance()
        {
            if(uObject!=null)
            {
                return UnityObject.Instantiate(uObject);
            }else
            {
                DebugLog.Error(AssetConst.LOGGER_NAME, "State is not finished or object is null");
                return null;
            }
        }

        protected internal override UnityObject GetInstance(UnityObject uObj)
        {
            if (uObj != null)
            {
                return UnityObject.Instantiate(uObj);
            }
            return null;
        }

        protected internal override bool IsAlive()
        {
            if (IsNeverDestroy)
            {
                return true;
            }
            return refCount > 0;
        }

        protected internal override bool IsDone()
        {
            return isDone;
        }

        protected internal override void Unload()
        {
            uObject = null;
            isDone = false;
        }
    }
}
#endif