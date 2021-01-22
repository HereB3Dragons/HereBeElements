using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IBeginDragHandler : IEventSystemHandler
    {
        void OnBeginDrag(PointerEventData eventData);
    }
}