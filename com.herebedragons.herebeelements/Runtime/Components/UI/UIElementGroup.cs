using System;
using HereBeElements.Internal;
using UnityEngine;

namespace HereBeElements.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElementGroup : UIElement
    {
        private UIElement[] _children;
        private CanvasGroup _cg;
    
        protected override void Awake()
        {
            base.Awake();
            _children = Utils.GetComponentsInChildren<UIElement>(this);
            _cg = GetComponent<CanvasGroup>();
        }

        public override void Show(bool onOff = true)
        {
            float a = onOff ? 1 : 0;
            if (GetCanvas().alpha != a)
                GetCanvas().alpha = a;
        }

        public override bool IsEnabled()
        {
            return GetCanvas().interactable;
        }

        public override void Enable(bool onOff = true)
        {
            if (GetCanvas().interactable != onOff)
                GetCanvas().interactable = onOff;
        }

        private CanvasGroup GetCanvas()
        {
            if (_cg == null)
                _cg = GetComponent<CanvasGroup>();
            return _cg;
        }


        public override void Highlight(bool onOff = true)
        {
            InvokeAll(el => el.Highlight(onOff));
            base.Highlight(onOff);
        }

        private void InvokeAll(Action<UIElement> action)
        {
            if (_children == null) return;
            foreach (UIElement child in _children)
                action.Invoke(child);
        }
    }
}