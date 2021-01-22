using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IPointerDownHandler : IEventSystemHandler
    {
        void OnPointerDown(PointerEventData eventData);
    }
}