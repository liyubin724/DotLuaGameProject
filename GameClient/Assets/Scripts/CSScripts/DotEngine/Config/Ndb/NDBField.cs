namespace DotEngine.Config.Ndb
{
    public enum NDBFieldType : int
    {
        Null = 0,

        Int = 1,
        IntArray = 101,
        
        Float = 2,
        FloatArray = 102,
        
        Long = 3,
        LongArray = 103,
        
        Bool = 4,
        BoolArray = 104,
        
        String = 5,
        StringArray = 105,
    }

    public class NDBField
    {
        public NDBFieldType type;
        public string name;
        public int offset;
    }
}
