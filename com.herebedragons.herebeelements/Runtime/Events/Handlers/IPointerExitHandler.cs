using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IPointerExitHandler : IEventSystemHandler
    {
        void OnPointerExit(PointerEventData eventData);
    }
}