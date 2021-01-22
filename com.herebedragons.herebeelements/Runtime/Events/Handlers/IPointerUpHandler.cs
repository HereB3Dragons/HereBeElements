using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IPointerUpHandler : IEventSystemHandler
    {
        void OnPointerUp(PointerEventData eventData);
    }
}