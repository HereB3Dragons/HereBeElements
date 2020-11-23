using HereBeElements.Internal;
using UnityEngine;

namespace HereBeElements.Templates
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElementGroup : UIElement
    {
        private CanvasGroup _cg;
        protected override void Awake()
        {
            base.Awake();
            _cg = GetComponent<CanvasGroup>();
            foreach (UIElement el in Utils.GetComponentsInChildren<UIElement>(this))
            {
                SelectEvent += el.Select;
                EnableEvent += () => el.Enable();
                DisableEvent += () => el.Disable();
                HighlightEvent += () => el.Highlight();
                DeHighlightEvent += () => el.DeHighlight();
            }
        }
    }
}