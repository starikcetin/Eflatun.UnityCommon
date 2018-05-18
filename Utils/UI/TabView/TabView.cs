using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Manager for a tabbed UI view.
    /// </summary>
    public class TabView : MonoBehaviour
    {
        [Header("Highlighter")]
        [SerializeField] private RectTransform _highlighter;
        [SerializeField] private bool _highlightOnX, _highlightOnY;

        [Header("Tabs")]
        [SerializeField] private List<HandleTabPair> _handleTabPairs;

        private void Awake()
        {
            foreach (var pair in _handleTabPairs)
            {
                pair.BindHandle(SwitchTab);
            }
        }

        /// <summary>
        /// Registers a new tab to this instance.
        /// Also binds handle to tab.
        /// </summary>
        public void Register(Panel tab, EventButton handle)
        {
            var newPair = new HandleTabPair(tab, handle);
            newPair.BindHandle(SwitchTab);
            _handleTabPairs.Add(newPair);
        }

        /// <summary>
        /// Unregisters a tab from this instance.
        /// Also unbinds handle from tab.
        /// </summary>
        public void Unregister(Panel tab)
        {
            var pair = _handleTabPairs.Single(a => a.Tab == tab);
            pair.UnbindHandle();
            _handleTabPairs.Remove(pair);
        }

        /// <summary>
        /// Switches the active tab.
        /// </summary>
        public void SwitchTab(Panel switchTo)
        {
            foreach (var pair in _handleTabPairs)
            {
                var tab = pair.Tab;

                if (tab == switchTo)
                {
                    tab.Show();

                    if (_highlightOnX || _highlightOnY)
                    {
                        Highlight(pair.Handle);
                    }
                }
                else
                {
                    tab.Hide();
                }
            }
        }

        private void Highlight(EventButton handle)
        {
            var position = new Vector2();

            if (_highlightOnX)
            {
                position.x = handle.transform.position.x;
            }

            if (_highlightOnY)
            {
                position.y = handle.transform.position.y;
            }

            _highlighter.position = position;
        }
    }
}
