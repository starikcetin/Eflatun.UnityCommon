using UnityEngine;
using UnityEngine.UI;

namespace Eflatun.UnityCommon.Utils.UI
{
    /// <summary>
    /// Base class for button interactions.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClickListener);
        }

        /// <summary>
        /// This will get invoked when button is clicked.
        /// </summary>
        protected abstract void OnClickListener();
    }
}
