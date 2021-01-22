using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IEndDragHandler : IEventSystemHandler
    {
        void OnEndDrag(PointerEventData eventData);
    }
}