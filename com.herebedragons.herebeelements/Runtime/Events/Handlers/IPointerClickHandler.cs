using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IPointerClickHandler : IEventSystemHandler
    {
        void OnPointerClick(PointerEventData eventData);
    }
}