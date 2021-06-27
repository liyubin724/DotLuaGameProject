namespace DotEngine.Config.NDB
{
    public enum NDBFieldType
    {
        None = 0,

        Bool = 'b',
        Int = 'i',
        Long = 'l',
        Float = 'f',
        String = 's',
        Text = 't',
    }

    public class NDBField
    {
        public NDBFieldType FieldType { get; set; } = NDBFieldType.None;
        public string Name { get; set; } = string.Empty;

        internal int Offset { get; set; } = 0;
    }

    public static class NDBFieldUtil
    {
        public static int GetFieldOffset(NDBFieldType fieldType)
        {
            if (fieldType == NDBFieldType.Bool)
            {
                return sizeof(bool);
            }
            else if (fieldType == NDBFieldType.Int || fieldType == NDBFieldType.String || fieldType == NDBFieldType.Text)
            {
                return sizeof(int);
            }
            else if (fieldType == NDBFieldType.Long)
            {
                return sizeof(long);
            }
            else if (fieldType == NDBFieldType.Float)
            {
                return sizeof(float);
            }
            else
            {
                return 0;
            }
        }
    }
}
