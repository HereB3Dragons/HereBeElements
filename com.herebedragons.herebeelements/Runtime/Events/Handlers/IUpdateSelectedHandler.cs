using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IUpdateSelectedHandler : IEventSystemHandler
    {
        void OnUpdateSelected(BaseEventData eventData);
    }
}