using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IPointerEnterHandler : IEventSystemHandler
    {
        void OnPointerEnter(PointerEventData eventData);
    }
}