namespace DotEngine.Config.Ndb
{
    public static class NDBConst
    {
        public static int GetFieldSize(NDBFieldType fieldType)
        {
            if (fieldType == NDBFieldType.Null)
            {
                return 0;
            }else  if(fieldType == NDBFieldType.Bool)
            {
                return sizeof(bool);
            }else if(fieldType == NDBFieldType.Float)
            {
                return sizeof(float);
            }else if(fieldType == NDBFieldType.Long)
            {
                return sizeof(long);
            }else
            {
                return sizeof(int);
            }
        }
    }
}
