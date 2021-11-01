using DotEngine.Pool;

namespace DotEngine.Net
{
    internal class ReceviedMessage : IPoolItem
    {
        public int Id { get; set; } = -1;
        public object Body { get; set; } = null;

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Id = -1;
            Body = null;
        }
    }
}
