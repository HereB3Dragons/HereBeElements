using System;
using System.Collections;
using UnityEngine;

namespace HereBeElements.Templates
{
    public interface IMoveable
    {
        IEnumerator MoveTo(Vector3 destination, Action callback = null, float duration = 1.5f, Action<float> progress = null);
    }
}