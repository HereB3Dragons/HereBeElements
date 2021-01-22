using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface ICancelHandler : IEventSystemHandler
    {
        void OnCancel(BaseEventData eventData);
    }
}