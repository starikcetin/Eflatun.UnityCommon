using System;
using UnityEngine;

namespace UnityCSCommon.Utils.UI
{
    [Serializable]
    public class HandleTabPair
    {
        [SerializeField] private Panel _tab;
        [SerializeField] private EventButton _handle;

        private Action _bindingAction;

        public Panel Tab
        {
            get { return _tab; }
        }

        public EventButton Handle
        {
            get { return _handle; }
        }

        public HandleTabPair(Panel tab, EventButton handle)
        {
            _tab = tab;
            _handle = handle;
        }

        public void BindHandle(Action<Panel> tabSwitcher)
        {
            _bindingAction = () => tabSwitcher(_tab);
            _handle.OnClick += _bindingAction;
        }

        public void UnbindHandle()
        {
            _handle.OnClick -= _bindingAction;
        }
    }
}
