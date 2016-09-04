using UnityEngine;
using UnityEngine.UI;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Tab representer used by <see cref="TabView"/>.
    /// </summary>
    [System.Serializable]
    public class TabViewTab
    {
        [SerializeField] private Button _handle;
        [SerializeField] private RectTransform _view;

        public Button Handle
        {
            get { return _handle; }
        }

        public RectTransform View
        {
            get { return _view; }
        }

        public void Show()
        {
            _view.gameObject.SetActive (true);
        }

        public void Hide()
        {
            _view.gameObject.SetActive (false);
        }

        public void Highlight (Graphic highlighter, bool highlighterX, bool highlighterY)
        {
            Vector2 newPos = highlighter.transform.position;

            if (highlighterX)
            {
                newPos.x = _handle.transform.position.x;
            }

            if (highlighterY)
            {
                newPos.y = _handle.transform.position.y;
            }

            highlighter.transform.position = newPos;
        }
    }
}