namespace DotEngine.Generic
{
    public class UniqueID
    {
        private long id = 0;

        public UniqueID()
        {
            id = 0;
        }

        public UniqueID(long start)
        {
            id = start;
        }

        public long NextID { get => ++id; }
        public long GetNextID()
        {
            return ++id;
        }

        private static UniqueID uniqueID = null;
        public static long ID
        {
            get
            {
                if(uniqueID == null)
                {
                    uniqueID = new UniqueID(); 
                }
                return uniqueID.NextID;
            }
        }
    }
}
