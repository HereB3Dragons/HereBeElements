using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IDropHandler : IEventSystemHandler
    {
        void OnDrop(PointerEventData eventData);
    }
}