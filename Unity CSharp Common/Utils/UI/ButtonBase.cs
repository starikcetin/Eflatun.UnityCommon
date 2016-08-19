using UnityEngine;
using UnityEngine.UI;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Base class for button interactions.
    /// </summary>
    [RequireComponent (typeof (Button))]
    public abstract class ButtonBase : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener (OnClick);
        }

        protected abstract void OnClick();
    }
}