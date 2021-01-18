using System;
using UnityEngine;

namespace HereBeElements.Animations
{
    [Serializable]
    public abstract class SimpleScaleAnimation<T> : ToggleAnimation<T> where T: Transform
    {
        public Vector3 destScale = Vector3.one;

        private Vector3 _originScale, _destScale;
        public override void Toggle(Action callback = null)
        {
            _originScale = GetTransform().localScale;
            _destScale = GetState() ? Vector3.one : destScale;
            base.Toggle(callback);
        }

        protected override void Animate(float progress)
        {
            GetTransform().localScale = Vector3.Lerp(_originScale, _destScale, 
                Mathf.SmoothStep(0, 1, progress));
        }
    }
}