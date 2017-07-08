namespace Common
{
    public class TSingleton<T> where T : class, new()
    {
        public static T Instance
        {
            get;
            private set;
        }

        static TSingleton()
        {
            if (TSingleton<T>.Instance == null)
            {
                TSingleton<T>.Instance = new T();
            }
        }

        public virtual void Clear()
        {
            TSingleton<T>.Instance = null;
            TSingleton<T>.Instance = new T();
        }

        public static T CreateInstance()
        {
            return TSingleton<T>.Instance;
        }
    }
}
