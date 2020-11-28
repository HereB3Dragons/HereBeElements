using UnityEngine;

namespace HereBeElements.Internal
{
    public interface ISelectable
    {
        GameObject GetGameObject();
        
        bool IsActive();
    }
}