using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IScrollHandler : IEventSystemHandler
    {
        void OnScroll(PointerEventData eventData);
    }
}