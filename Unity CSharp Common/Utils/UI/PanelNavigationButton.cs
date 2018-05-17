using System.Collections.Generic;
using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// A Button type that navigates between two panels when clicked.
    /// </summary>
    public class PanelNavigationButton : ButtonBase
    {
        [SerializeField] private List<Panel> _hideOnClick;
        [Space]
        [SerializeField] private List<Panel> _showOnClick;

        protected override void OnClickListener()
        {
            foreach (Panel panel in _hideOnClick)
            {
                panel.Hide();
            }

            foreach (Panel panel in _showOnClick)
            {
                panel.Show();
            }
        }
    }
}
