using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface ISubmitHandler : IEventSystemHandler
    {
        void OnSubmit(BaseEventData eventData);
    }
}