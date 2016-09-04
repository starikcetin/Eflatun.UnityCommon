using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Manager for a tabbed UI view, with tabs being <see cref="TabViewTab"/> components.
    /// </summary>
    public class TabView : MonoBehaviour
    {
        [Header ("Highlighter")]
        [SerializeField] private Graphic _highlighter;
        [SerializeField] private bool _highlighterX, _highlighterY;

        [Header ("Tabs")]
        [SerializeField] private List<TabViewTab> _tabs;

        public Graphic Highlighter
        {
            get { return _highlighter; }
        }

        public bool HighlighterX
        {
            get { return _highlighterX; }
        }

        public bool HighlighterY
        {
            get { return _highlighterY; }
        }

        public List<TabViewTab> Tabs
        {
            get { return _tabs; }
        }

        void Start()
        {
            // Do not convert this statement into a foreach loop. Variable capturing does not work properly for a foreach loop.
            _tabs.ForEach (tab => tab.Handle.onClick.AddListener (() => ChangeTab (tab)));

            ChangeTab (_tabs[0]);
        }

        public void ChangeTab (TabViewTab toShow)
        {
            _tabs.ForEach (a => a.Hide());
            toShow.Show();
            if (_highlighter) toShow.Highlight (_highlighter, _highlighterX, _highlighterY);
        }
    }
}