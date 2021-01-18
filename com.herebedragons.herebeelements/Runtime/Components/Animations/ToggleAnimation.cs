using System;
using BetBluff.Table.interfaces;
using UnityEngine;

namespace HereBeElements.Animations
{
    public abstract class ToggleAnimation<T>: SimpleAnimation<T>, IToggle where T : Transform
    {
        private bool _state;
        private Coroutine _anim;
        public float duration = 1.5f;

        public delegate void ToggleHandler();

        public ToggleHandler OnToggleOn;
        public ToggleHandler OnToggleOff;
        public virtual void Toggle(Action callback = null)
        {
            if (IsAnimating())
                return;
            HandleEvents();
            _anim = StartCoroutine(Animate(f => Animate(f),
                () =>
                {
                    _state = !_state;
                    if (callback != null)
                        callback.Invoke();
                },
                duration));
        }

        private void HandleEvents()
        {
            if (!_state && OnToggleOn != null)
                OnToggleOn.Invoke();
            if (_state && OnToggleOff != null)
                OnToggleOff.Invoke();
        }

        public void Interrupt()
        {
            if (_anim != null)
                StopCoroutine(_anim);
        }

        protected abstract void Animate(float progress);

        protected bool GetState()
        {
            return _state;
        }
    }
}