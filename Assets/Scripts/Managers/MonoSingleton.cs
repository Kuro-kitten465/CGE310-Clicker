using UnityEngine;

namespace KuroNeko.Utilities.DesignPattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static readonly object _lock = new();

        protected MonoSingleton() { }

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject singleton = new(typeof(T).Name);
                            _instance = singleton.AddComponent<T>();
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}