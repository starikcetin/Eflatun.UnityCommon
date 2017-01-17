using UnityEngine;

namespace UnityCSCommon.Utils.CodePatterns
{
    /// <summary>
    /// <para>Base class for a singleton MonoBehaviour that stays alive until application death even if current scene changes.</para>
    /// <para>If you want to use Awake or OnApplicationQuit methods in implementator, use 'new' keyword in method signature
    /// and call 'base.Awake()' or 'base.OnApplicationQuit()' in the first line of your method.</para>
    /// <para>Be aware this will not prevent a non singleton constructor such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// This type inherits from MonoBehaviour so we can use Coroutines.</para>
    /// </summary>
    public class GlobalSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static readonly object Lock = new object();

        protected GlobalSingleton() { }

        /// <summary>
        /// Returns the singleton instance of <see cref="T"/>.
        /// </summary>
        public static T Instance
        {
            get
            {
                return Instance_Get();
            }
        }

        private static T Instance_Get()
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning(string.Format("[GlobalSingleton] Application is quitting! Returning null instead of '{0}'.", typeof(T)));
                return null;
            }

            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError(string.Format("[GlobalSingleton] There is more than one {0} in the scene! Reopening the scene might fix it.", typeof(T)));
                    }
                    else if (_instance == null)
                    {
                        GameObject newContainer = new GameObject();
                        _instance = newContainer.AddComponent<T>();
                        newContainer.name = string.Format ("(global singleton) {0}", typeof(T));

                        Debug.Log(string.Format("[GlobalSingleton] An instance of {0} is needed in the scene, so '{1}' was created.", typeof(T), newContainer));
                    }
                    else
                    {
                        Debug.Log(string.Format("[GlobalSingleton] Using instance already created: '{0}' on '{1}'.", typeof(T), _instance.gameObject));
                    }
                }

                return _instance;
            }
        }

        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// If any script calls Instance after Singleton have been destroyed,
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// When we load a scene that has a GameObject with our singleton on it as a component, singleton
        /// gets duplicated because we already have a singleton instance with DontDestroyOnLoad.
        /// To prevent this, we must destroy the instance that is created while scene load if we
        /// already have an instance. Lock was not necessary, but feels safe.
        /// </summary>
        protected void Awake()
        {
            lock (Lock)
            {
                if (_instance != null)
                {
                    Debug.LogWarning(string.Format("[GlobalSingleton] Destroying duplicate of '{0}' on '{1}'.", typeof(T), gameObject));
                    DestroyImmediate(this, false);
                    return;
                }

                DontDestroyOnLoad (Instance_Get());
                Debug.Log(string.Format("[GlobalSingleton] DontDestroyOnLoad called for '{0}' on '{1}'.", typeof(T), gameObject));
            }
        }
    }
}