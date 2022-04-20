using SystemObject = System.Object;

namespace DotEngine.NAssets
{
    internal class AssetAsyncRequest
    {
        public int Id { get; set; } = -1;
        public string[] Addresses { get; set; }
        public string[] Paths { get; set; }
        public bool IsInstance { get; set; }
        public AssetAsyncProgress OnProgress;
        public AssetAsyncComplete OnComplete;
        public AssetBatchAsyncProgress OnBatchProgress;
        public AssetBatchAsyncComplete OnBatchComplete;
        public AssetAsyncPriority Priority { get; set; } = AssetAsyncPriority.Default;
        public SystemObject Userdata { get; set; }

        public void DoRelease()
        {
            Id = -1;
            Addresses = null;
            Paths = null;
            IsInstance = false;
            OnProgress = null;
            OnComplete = null;
            OnBatchComplete = null;
            OnBatchProgress = null;
            Priority = AssetAsyncPriority.Default;
            Userdata = null;
        }
    }
}
