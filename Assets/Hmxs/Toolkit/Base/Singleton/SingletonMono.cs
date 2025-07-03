using UnityEngine;

namespace Hmxs.Toolkit.Base.Singleton
{
    /// <summary>
    /// 泛型单例基类-继承Mono
    /// </summary>
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T _instance;
        private static readonly object _lock = new();
        private static bool _isApplicationQuitting;

        /// <summary>
        /// Global access point to the singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting)
                    return null;

                if (_instance) return _instance;
                lock (_lock)
                {
                    if (_instance) return _instance;
                    var singletonObject = new GameObject($"{typeof(T).Name}_SingletonMono");
                    _instance = singletonObject.AddComponent<T>();
                    Debug.Log($"{typeof(T).Name}_SingletonMono has been created.");
                }
                return _instance;
            }
        }

        /// <summary>
        /// Override this to control if the singleton survives scene changes.
        /// </summary>
        protected virtual bool KeepAliveAcrossScenes => true;

        protected virtual void Awake()
        {
            if (!_instance)
            {
                _instance = this as T;
                if (KeepAliveAcrossScenes) DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[{typeof(T)}] Duplicate instance detected. Destroying {name}.");
                Destroy(gameObject);
                return;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
            if (_instance == this) _instance = null;
        }
    }
}
