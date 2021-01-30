namespace DotEngine.Generic
{
    public class UniqueIntID
    {
        private int id = 0;
        public UniqueIntID()
        {
        }

        public UniqueIntID(int start)
        {
            id = start;
        }

        public int NextID { get => ++id; }
        public int GetNextID()
        {
            return ++id;
        }

        private static UniqueIntID uniqueIntID = null;
        public static int ID
        {
            get
            {
                if (uniqueIntID == null)
                {
                    uniqueIntID = new UniqueIntID();
                }
                return uniqueIntID.NextID;
            }
        }
    }
}
