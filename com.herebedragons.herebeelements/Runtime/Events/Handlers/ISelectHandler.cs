using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface ISelectHandler : IEventSystemHandler
    {
        void OnSelect(BaseEventData eventData);
    }
}