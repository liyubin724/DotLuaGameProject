namespace DotEngine.Core
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance = null;
        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = new T();
                instance.OnInit();
            }
            return instance;
        }

        public static void DestroyInstance()
        {
            if (instance != null)
            {
                instance.OnDestroy();
                instance = null;
            }
        }

        protected Singleton()
        { }

        protected virtual void OnInit()
        { }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}
