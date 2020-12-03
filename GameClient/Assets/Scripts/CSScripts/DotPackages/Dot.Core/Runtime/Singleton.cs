
namespace DotEngine
{
    public abstract class Singleton <T> where T :Singleton<T>, new()
    {
        protected static T instance = null;
        public static T GetInstance()
        {
            if(instance == null)
            {
                instance = new T();
                instance.DoInit();
            }
            return instance;
        }

        protected virtual void DoInit()
        {
        }

        public virtual void DoDispose()
        {
            instance = null;
        }
    }
}

