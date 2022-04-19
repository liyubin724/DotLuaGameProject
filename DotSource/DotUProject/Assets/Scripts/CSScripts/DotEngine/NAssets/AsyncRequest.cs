using SystemObject = System.Object;

namespace DotEngine.NAssets
{
    internal class AsyncRequest
    {
        public int Id { get; set; }
        public string[] Addresses { get; set; }
        public string[] Paths { get; set; }
        public bool IsInstance { get; set; }
        public AssetAsyncProgress OnProgress;
        public AssetAsyncComplete OnComplete;
        public AssetBatchAsyncProgress OnBatchProgress;
        public AssetBatchAsyncComplete OnBatchComplete;
        public AssetAsyncPriority Priority { get; set; }
        public SystemObject Userdata { get; set; }
    }
}
