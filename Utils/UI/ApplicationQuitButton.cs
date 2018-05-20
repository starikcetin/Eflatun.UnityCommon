using UnityEngine;

namespace Eflatun.UnityCommon.Utils.UI
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