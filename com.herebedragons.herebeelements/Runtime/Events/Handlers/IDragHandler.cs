using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IDragHandler : IEventSystemHandler
    {
        void OnDrag(PointerEventData eventData);
    }
}