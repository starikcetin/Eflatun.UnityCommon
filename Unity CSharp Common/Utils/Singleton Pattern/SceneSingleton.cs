using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCSharpCommon.Utils.SingletonPatterns
{
    /// <summary>
    /// <para>Base class for a singleton MonoBehaviour that gets destroyed if current scene changes.</para>
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
                string sceneName = SceneManager.GetActiveScene().name;

                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[SceneSingleton (" + sceneName + ")] Instance '" + typeof (T) + "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T) FindObjectOfType(typeof (T));

                        if (FindObjectsOfType(typeof (T)).Length > 1)
                        {
                            Debug.LogError("[SceneSingleton (" + sceneName + ")] Something went really wrong " +
                                           " - there should never be more than 1 singleton!" +
                                           " Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof (T);

                            Debug.Log("[SceneSingleton (" + sceneName + ")] An instance of " + typeof (T) + " is needed in the scene, so '" +
                                      singleton + "' was created.");
                        }
                        else
                        {
                            Debug.Log("[SceneSingleton (" + sceneName + ")] Using instance already created: " + _instance.gameObject.name);
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
        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}