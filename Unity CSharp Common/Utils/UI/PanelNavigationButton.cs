using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// A Button type that navigates between two panels when clicked.
    /// </summary>
    public class PanelNavigationButton : ButtonBase
    {
        [SerializeField] private Panel _hideOnClick;
        [SerializeField] private Panel _showOnClick;

        protected override void OnClick()
        {
            if (_hideOnClick) _hideOnClick.Hide();
            if (_showOnClick) _showOnClick.Show();
        }
    }
}