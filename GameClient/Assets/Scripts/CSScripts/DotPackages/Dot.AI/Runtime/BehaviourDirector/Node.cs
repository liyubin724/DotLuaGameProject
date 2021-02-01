namespace DotEngine.AI.BD
{
    public abstract class Node
    {
        public int UniqueID = -1;

        public virtual bool IsActive()
        {
            return true;
        } 
    }
}