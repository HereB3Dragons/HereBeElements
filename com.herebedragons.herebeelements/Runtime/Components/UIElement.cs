using System;
using System.Collections;
using HereBeElements.Templates;
using HereBeElements.Internal;
using HereBeElements.Shaders;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HereBeElements
{

    [Serializable]
    [RequireComponent(typeof(ShaderControl))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIElement : UISelectable, IPointerClickHandler, IElement
    {
        protected ShaderControl _sc;
        protected Graphic _graphic;
        protected bool _isHighlight = false;
        protected bool _isVisible = true;
        protected RectTransform _transform;

        public bool IsVisible()
        {
            return _isVisible;
        }

        public virtual void Show(bool onOff = true)
        {
            if (_sc != null)
                _sc.Opacity = onOff ? 1 : 0;
            _isHighlight = onOff;
        }

        public virtual void Hide()
        {
            Show(false);
        }

        public bool IsEnabled()
        {
            return IsInteractable();
        }

        public virtual void Enable(bool onOff = true)
        {
            this.interactable = onOff;
        }

        public virtual void Disable()
        {
            Enable(false);
        }

        public bool IsHighlight()
        {
            return base.IsHighlighted() || _isHighlight;
        }

        public virtual void Highlight(bool onOff = true)
        {
            if (onOff)
            {
                HighlightEventHandler highlight = HighlightEvent;
                if (highlight != null)
                {
                    UISystemProfilerApi.AddMarker("UIElement.onHighlight", this);
                    highlight();
                    _isHighlight = true;
                }
            }
            else
            {
                DeHighlightEventHandler dehighlight = DeHighlightEvent;
                if (dehighlight != null)
                {
                    UISystemProfilerApi.AddMarker("UIElement.onDeHighlight", this);
                    dehighlight();
                    _isHighlight = false;
                }
            }
        }

        public virtual void DeHighlight()
        {
            Highlight(false);
        }

        protected override void Awake()
        {
            base.Awake();
            _sc = GetComponent<ShaderControl>();
            _graphic = GetComponent<Graphic>();
            _transform = GetComponent<RectTransform>();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (_sc == null)
                _sc = GetComponent<ShaderControl>();
            base.OnValidate();
            if (_sc != null)
                ApplyShaderConfig();
        }
#endif
        public void ApplyShaderConfig()
        {
            _sc.ApplyConfig();
        }

        public Graphic GetGraphic()
        {
            if (_graphic == null)
                _graphic = GetComponent<Graphic>();
            return _graphic;
        }


        public delegate void EnableEventHandler();

        public event EnableEventHandler EnableEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            EnableEventHandler enable = EnableEvent;
            if (enable != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onEnable", this);
                enable();
            }
        }

        public delegate void DisableEventHandler();

        public event DisableEventHandler DisableEvent;

        protected override void OnDisable()
        {
            base.OnDisable();
            DisableEventHandler disable = DisableEvent;
            if (disable != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onDisable", this);
                disable();
            }
        }

        public delegate void HighlightEventHandler();

        public event HighlightEventHandler HighlightEvent;

        public delegate void DeHighlightEventHandler();

        public event DeHighlightEventHandler DeHighlightEvent;

        public delegate void SelectEventHandler();

        public event SelectEventHandler SelectEvent;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectEventHandler select = SelectEvent;
            if (select != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onSelect", this);
                select();
            }
        }

        public delegate void DeselectEventHandler();

        public event DeselectEventHandler DeselectEvent;

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            DeselectEventHandler deselect = DeselectEvent;
            if (deselect != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onDeselect", this);
                deselect();
            }
        }

        public delegate void MouseEnterEventHandler();

        public event MouseEnterEventHandler MouseEnterEvent;

        public delegate void MouseLeaveEventHandler();

        public event MouseLeaveEventHandler MouseLeaveEvent;

        public delegate void ClickEventHandler();

        public event ClickEventHandler ClickEvent;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            ClickEventHandler click = ClickEvent;
            if (click != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onClick", this);
                click();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            MouseEnterEventHandler mouseEnter = MouseEnterEvent;
            if (mouseEnter != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onMouseEnter", this);
                mouseEnter();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            MouseLeaveEventHandler mouseLeave = MouseLeaveEvent;
            if (mouseLeave != null)
            {
                UISystemProfilerApi.AddMarker("UIElement.onMouseLeave", this);
                mouseLeave();
            }
        }

        public IEnumerator MoveTo(Vector3 destination, Action callback = null, float duration = 1.5f, Action<float> progress = null)
        {
            if (_transform == null)
                yield break;
            
            Vector3 origin = _transform.localPosition;
            float journey = 0f;

            while (journey <= duration)
            {
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey / duration);
                if (_transform == null) break;
                _transform.localPosition = Vector3.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, percent));
                if (progress != null)
                    progress.Invoke(percent);
                yield return null;
            }
            
            if (callback != null)
                callback.Invoke();
        }
    }
}
