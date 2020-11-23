
using HereBeElements.Internal;

namespace HereBeElements.Templates
{
    public class ElementGroup: Element
    {
        protected override void Awake()
        {
            base.Awake();
            foreach (Element el in Utils.GetComponentsInChildren<Element>(this))
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