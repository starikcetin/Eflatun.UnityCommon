using UnityEngine;

namespace starikcetin.UnityCommon.Utils.UI
{
    /// <summary>
    /// A Button that quits the application when clicked.
    /// </summary>
    public class ApplicationQuitButton : ButtonBase
    {
        protected override void OnClickListener()
        {
            Application.Quit();
        }
    }
}