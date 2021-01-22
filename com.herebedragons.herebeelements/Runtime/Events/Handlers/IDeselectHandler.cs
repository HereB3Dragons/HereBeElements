using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IDeselectHandler : IEventSystemHandler
    {
        void OnDeselect(BaseEventData eventData);
    }
}