using UnityCSCommon.Utils.Common;
using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// A component to identify and interfere with Panel gameObjects.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        /// <summary>
        /// Activates the panel <see cref="GameObject"/> and all of it's ancestors.
        /// </summary>
        public void Show()
        {
            OnBeforeShow();
            gameObject.SetActiveWithAncestors (true);
        }

        /// <summary>
        /// Deactivates the panel <see cref="GameObject"/>.
        /// (Only the panel, does nothing with ancestors.)
        /// </summary>
        public void Hide()
        {
            OnBeforeHide();
            gameObject.SetActive (false);
        }

        /// <summary>
        /// Called just before showing panel.
        /// </summary>
        protected virtual void OnBeforeShow () {}

        /// <summary>
        /// Called just before hiding panel.
        /// </summary>
        protected virtual void OnBeforeHide () {}
    }
}