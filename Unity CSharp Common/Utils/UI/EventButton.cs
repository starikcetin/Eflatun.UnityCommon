using System;
using UnityCSCommon.Utils.Common;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Has an <see cref="OnClick"/> event that will get invoked when button is clicked.
    /// </summary>
    public class EventButton : ButtonBase
    {
        /// <summary>
        /// This will get invoked when button is clicked.
        /// </summary>
        public event Action OnClick;

        #region Overrides of ButtonBase

        protected override void OnClickListener()
        {
            OnClick.InvokeSafe();
        }

        #endregion
    }
}
