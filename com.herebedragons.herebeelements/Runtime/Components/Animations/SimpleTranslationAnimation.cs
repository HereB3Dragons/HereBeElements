using System;
using HereBeElements.Util;
using UnityEngine;

namespace HereBeElements.Animations
{
    public abstract class SimpleTranslationAnimation<T>: ToggleAnimation<T> where T: Transform
    {
        public Direction direction = Direction.BottomToTop;
        [Range(-1f,1f)]
        public float buffer = 0f;

        private Vector3 _origin, _destination;

        public override void Toggle(Action callback = null)
        {
            _origin = GetTransform().localPosition;
            _destination = GetDestinationPosition(direction);
            base.Toggle(callback);
        }

        protected override void Animate(float progress)
        {
            GetTransform().localPosition = Vector3.Lerp(_origin, _destination, 
                Mathf.SmoothStep(0.0f, 1.0f, progress));
        }

        protected abstract Vector3 GetDestinationPosition(Direction d);
    }
}