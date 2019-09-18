using UnityEngine;

namespace starikcetin.Eflatun.UnityCommon.Utils.UI
{
    /// <summary>
    /// A component to identify and interfere with Panel gameObjects.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        /// <summary>
        /// Activates the panel <see cref="GameObject"/>.
        /// </summary>
        public void Show()
        {
            OnBeforeShow();
            gameObject.SetActive(true);
            OnAfterShow();
        }

        /// <summary>
        /// Deactivates the panel <see cref="GameObject"/>.
        /// </summary>
        public void Hide()
        {
            OnBeforeHide();
            gameObject.SetActive(false);
            OnAfterHide();
        }

        /// <summary>
        /// Called just before showing panel.
        /// </summary>
        protected virtual void OnBeforeShow()
        {
        }

        /// <summary>
        /// Called just after showing panel.
        /// </summary>
        protected virtual void OnAfterShow()
        {
        }

        /// <summary>
        /// Called just before hiding panel.
        /// </summary>
        protected virtual void OnBeforeHide()
        {
        }

        /// <summary>
        /// Called just after hiding panel.
        /// </summary>
        protected virtual void OnAfterHide()
        {
        }
    }
}
