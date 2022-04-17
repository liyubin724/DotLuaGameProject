using SystemObject = System.Object;

namespace DotEngine.NAssets
{
    public class AssetRequest
    {
        public int Id { get; set; }
        public string[] Addresses { get; set; }
        public string[] Paths { get; set; }
        public bool IsInstance { get; set; }
        public AssetProgress OnProgress;
        public AssetComplete OnComplete;
        public AssetBatchProgress OnBatchProgress;
        public AssetBatchComplete OnBatchComplete;
        public AssetPriority Priority { get; set; }
        public SystemObject Userdata { get; set; }
    }
}
