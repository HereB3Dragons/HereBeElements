using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IInitializePotentialDragHandler : IEventSystemHandler
    {
        void OnInitializePotentialDrag(PointerEventData eventData);
    }
}