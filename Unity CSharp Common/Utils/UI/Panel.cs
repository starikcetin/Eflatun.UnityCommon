using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// A component to identify and interfere with Panel gameobjects.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}