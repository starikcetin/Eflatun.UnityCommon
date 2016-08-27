using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCSCommon.Utils.SingletonPatterns
{
    /// <summary>
    /// <para>Base class for a singleton MonoBehaviour that gets destroyed if current scene changes.</para>
    /// <para>If you want to use Awake or OnApplicationQuit methods in implementator, use 'new' keyword in method signature
    /// and call 'base.Awake()' or 'base.OnApplicationQuit()' in the first line of your method.</para>
    /// <para>Be aware this will not prevent a non singleton constructor such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// This type inherits from MonoBehaviour so we can use Coroutines.</para>
    /// </summary>
    public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static readonly object Lock = new object();

        protected SceneSingleton() { }

        /// <summary>
        /// Returns the singleton instance of <see cref="T"/> for this scene. Please double check you are in the correct scene before calling this.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning(string.Format("[SceneSingleton ({0})] Application is quitting! Returning null instead of '{1}'.", SceneManager.GetActiveScene().name, typeof(T)));
                    return null;
                }

                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError(string.Format("[SceneSingleton ({0})] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.", SceneManager.GetActiveScene().name));
                        }
                        else if (_instance == null)
                        {
                            GameObject container = new GameObject();
                            _instance = container.AddComponent<T>();
                            container.name = string.Format("(scene singleton) {0}", typeof(T));

                            Debug.Log(string.Format("[SceneSingleton ({0})] An instance of {1} is needed in the scene, so '{2}' was created.", SceneManager.GetActiveScene().name, typeof(T), container));
                        }
                        else
                        {
                            Debug.Log(string.Format("[SceneSingleton ({0})] Using instance already created: {1}", SceneManager.GetActiveScene().name, _instance.gameObject.name));
                        }
                    }

                    return _instance;
                }
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
        /// When we load a scene -that has a GameObject with our singleton on it as a component- with
        /// LoadLevelAdditive, singleton gets duplicated because we already have a singleton instance
        /// and the current scene objects will not get destroyed since we loaded other scene "additive".
        /// To prevent this, we must destroy the instance that is created while "additive" scene load
        /// if we already have an instance. Lock was not necessary, but feels safe.
        /// </summary>
        protected void Awake()
        {
            lock (Lock)
            {
                if (_instance != null)
                {
                    Debug.LogWarning(string.Format("[SceneSingleton ({0})] Destroying duplicate of '{1}' on '{2}'.", SceneManager.GetActiveScene().name, typeof(T), gameObject.name));
                    DestroyImmediate(this, false);
                }
            }
        }
    }
}