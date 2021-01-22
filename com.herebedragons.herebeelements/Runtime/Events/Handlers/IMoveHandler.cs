using UnityEngine.EventSystems;

namespace HereBeElements.Events.Handlers
{
    public interface IMoveHandler : IEventSystemHandler
    {
        void OnMove(AxisEventData eventData);
    }
}