using System;
using System.Collections;
using BetBluff.Table.interfaces;
using UnityEngine;

namespace HereBeElements.Animations
{
    [Serializable]
    public abstract class SimpleAnimation<T>: MonoBehaviour, IAnimatable where T : Transform
    {
        private bool _isAnimating = false;
        private T _transform;

        private void Awake()
        {
            _transform = transform as T;
        }

        protected T GetTransform()
        {
            return _transform;
        }

        public virtual IEnumerator MoveTo(Vector3 destination, Action callback = null, float duration = 1.5f, Action<float> progress = null)
        {
            Vector3 origin = GetTransform().localPosition;
            return Animate((f) =>
            {
                GetTransform().localPosition = Vector3.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, f));
                if (progress != null)
                    progress.Invoke(f);
            }, callback, duration);
        }
        
        public IEnumerator Animate(Action<float> anim, Action callback = null, float duration = 1.5f)
        {
            SetAnimating(true);
            if (_transform == null || anim == null)
                yield break;
            
            float journey = 0f;

            while (journey <= duration)
            {
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey / duration);
                
                if (anim != null)
                    anim.Invoke(percent);
               
                yield return null;
            }
            
            SetAnimating(false);
            
            if (callback != null)
                callback.Invoke();
        }
        
        public virtual bool IsAnimating()
        {
            return _isAnimating;
        }

        public virtual void SetAnimating(bool value)
        {
            _isAnimating = value;
        }
    }
}