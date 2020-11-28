using System;
using HereBeElements.Internal;

namespace HereBeElements
{
    public class ElementGroup: Element
    {
        private Element[] _children;
    
        protected override void Awake()
        {
            base.Awake();
            _children = Utils.GetComponentsInChildren<Element>(this);
        }

        public override void Show(bool onOff = true)
        {
            InvokeAll(el => el.Show(onOff));
            base.Show(onOff);
        }

        public override void Enable(bool onOff = true)
        {
            InvokeAll(el => el.Show(onOff));
            base.Show(onOff);
        }


        public override void Highlight(bool onOff = true)
        {
            InvokeAll(el => el.Highlight(onOff));
            base.Highlight(onOff);
        }

        private void InvokeAll(Action<Element> action)
        {
            if (_children == null) return;
            foreach (Element child in _children)
                action.Invoke(child);
        }
    }
}