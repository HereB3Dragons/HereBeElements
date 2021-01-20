using System.Collections.Generic;
using HereBeElements.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HereBeElements.UI
{
    public class UISlider: UIElementGroup, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
    {
        public delegate void SliderChangeHandler(float value);
        public SliderChangeHandler SliderChangeEvent;

        public float step = 1;

        [SerializeField]
        private RectTransform m_HandleRect;

        /// <summary>
        /// Optional RectTransform to use as a handle for the slider.
        /// </summary>
        public RectTransform handleRect { get { return m_HandleRect; } set {m_HandleRect = value; } }
        
        [Space]

        [SerializeField]
        private Direction m_Direction = Direction.LeftToRight;
        
        public Direction direction { get { return m_Direction; }
            set { m_Direction = value; }
        }
        
        [SerializeField]
        private float m_MinValue = 0;

        /// <summary>
        /// The minimum allowed value of the slider.
        /// </summary>
        public float minValue { get { return m_MinValue; }
            set { m_MinValue = value; }
        }

        [SerializeField]
        private float m_MaxValue = 1;

        /// <summary>
        /// The maximum allowed value of the slider.
        /// </summary>
        public float maxValue { get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }

        [SerializeField]
        private bool m_WholeNumbers = false;

        /// <summary>
        /// Should the value only be allowed to be whole numbers?
        /// </summary>
        public bool wholeNumbers { get { return m_WholeNumbers; }
            set { m_WholeNumbers = value; }
        }
        
        [SerializeField]
        protected float m_Value;

        /// <summary>
        /// The current value of the slider.
        /// </summary>
        public virtual float value
        {
            get
            {
                if (wholeNumbers)
                    return Mathf.Round(m_Value);
                return m_Value;
            }
            set
            {
                Set(value);
            }
        }

        /// <summary>
        /// The current value of the slider normalized into a value between 0 and 1.
        /// </summary>
        public float normalizedValue
        {
            get
            {
                if (Mathf.Approximately(minValue, maxValue))
                    return 0;
                return Mathf.InverseLerp(minValue, maxValue, value);
            }
            set
            {
                this.value = Mathf.Lerp(minValue, maxValue, value);
            }
        }
        
        // Private fields

        private Transform m_HandleTransform;
        private RectTransform m_HandleContainerRect;

        // The offset from handle position to mouse down position
        private Vector2 m_Offset = Vector2.zero;

        private DrivenRectTransformTracker m_Tracker;

        // This "delayed" mechanism is required for case 1037681.
        private bool m_DelayedUpdateVisuals = false;

        // Size of each step.
        private float stepSize { get; set; }
        
        protected UISlider()
        {}

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            
            if (wholeNumbers)
            {
                m_MinValue = Mathf.Round(m_MinValue);
                m_MaxValue = Mathf.Round(m_MaxValue);
            }
            
            //Onvalidate is called before OnEnabled. We need to make sure not to touch any other objects before OnEnable is run.
            if (IsActive())
            {
                UpdateCachedReferences();
                // Update rects in next update since other things might affect them even if value didn't change.
                m_DelayedUpdateVisuals = true;
            }

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif 

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                Set(m_Value, false);
#endif
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateCachedReferences();
            Set(m_Value, false);
            UpdateVisuals();
        }
        
        /// <summary>
        /// Update the rect based on the delayed update visuals.
        /// Got around issue of calling sendMessage from onValidate.
        /// </summary>
        protected virtual void Update()
        {
            if (m_DelayedUpdateVisuals)
            {
                m_DelayedUpdateVisuals = false;
                Set(m_Value, false);
                UpdateVisuals();
            }
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Has value changed? Various elements of the slider have the old normalisedValue assigned, we can use this to perform a comparison.
            // We also need to ensure the value stays within min/max.
            m_Value = ClampValue(m_Value);
           
            UpdateVisuals();

        }
        
          void UpdateCachedReferences()
        {
            if (m_HandleRect && m_HandleRect != (RectTransform)transform)
            {
                m_HandleTransform = m_HandleRect.transform;
                m_HandleContainerRect = (RectTransform)m_HandleTransform;
            }
            else
            {
                m_HandleRect = null;
                m_HandleContainerRect = null;
            }
        }
          
        enum Axis
        {
            Horizontal = 0,
            Vertical = 1
        }

        Axis axis { get { return (m_Direction == Direction.LeftToRight || m_Direction == Direction.RightToLeft) ? Axis.Horizontal : Axis.Vertical; } }

        bool reverseValue
        {
            get { return m_Direction == Direction.RightToLeft; }
        }


// Force-update the slider. Useful if you've changed the properties and want it to update visually.
        public void UpdateVisuals()
      {
#if UNITY_EDITOR
          if (!Application.isPlaying)
              UpdateCachedReferences();
#endif

          m_Tracker.Clear();

          if (m_HandleRect != null)
          {
              m_Tracker.Add(this, m_HandleRect, DrivenTransformProperties.Pivot);
              m_HandleRect.pivot = new Vector2(NormalizeValue(m_Value), 0.5f);
          }
      }
      
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            //This can be invoked before OnEnabled is called. So we shouldn't be accessing other objects, before OnEnable is called.
            if (!IsActive())
                return;

            UpdateVisuals();
        }

        float ClampValue(float input)
        {
            float newValue = Mathf.Clamp(input, minValue, maxValue);
            if (wholeNumbers)
                newValue = Mathf.Round(newValue);
            return newValue;
        }

        /// <summary>
        /// Set the value of the slider.
        /// </summary>
        /// <param name="input">The new value for the slider.</param>
        /// <param name="sendEvent">If the OnValueChanged callback should be invoked.</param>
        /// <remarks>
        /// Process the input to ensure the value is between min and max value. If the input is different set the value and send the callback is required.
        /// </remarks>
        public virtual void Set(float input,  bool sendEvent = true)
        {
            // Clamp the input
            float newValue = ClampValue(input);

            if (sendEvent && (int)newValue != (int)m_Value)
            {
                if (SliderChangeEvent != null)
                    SliderChangeEvent.Invoke(newValue);
            } 
            
            m_Value = newValue;
            UpdateVisuals();
           
        }

        public virtual void Step(float step)
        {
            Set(m_Value + step);
        }
        
        private float NormalizeValue(float value)
        {
            if (value > m_MaxValue) value = m_MaxValue;
            if (value < m_MinValue) value = m_MinValue;

            float val = (value - m_MinValue) / (m_MaxValue - m_MinValue);

            return reverseValue ? 1 - val : val;
        }
        
        private Dictionary<string, ClickEventHandler> _slider_clicks = new Dictionary<string, ClickEventHandler>();
        protected override void Subscribe(bool onOff = true)
        {
            base.Subscribe(onOff);
            if (onOff)
            {
                foreach (SliderButton b in GetComponentsInChildren<SliderButton>())
                {
                    ClickEventHandler handler = () => Step(b.GetDelta());
                    _slider_clicks[b.name] = handler;
                    b.ClickEvent += handler;
                }
            }
            else
            {
                foreach (SliderButton b in GetComponentsInChildren<SliderButton>())
                    if (_slider_clicks.ContainsKey(b.name))
                        b.ClickEvent -= _slider_clicks[b.name];
                    
                _slider_clicks.Clear();    
            }
        }

#if UNITY_EDITOR
        protected override void OnDisable()
        {
            m_Tracker.Clear();
            base.OnDisable();
        }
#endif

        /// <summary>
        /// See ICanvasElement.LayoutComplete
        /// </summary>
        public virtual void LayoutComplete()
        {}

        /// <summary>
        /// See ICanvasElement.GraphicUpdateComplete
        /// </summary>
        public virtual void GraphicUpdateComplete()
        {}
        
        // Update the slider's position based on the mouse.
        void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            RectTransform clickRect = m_HandleRect;
            if (clickRect != null && clickRect.rect.size[(int)axis] > 0)
            {
                Vector2 localCursor;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(clickRect, eventData.position, cam, out localCursor))
                    return;
                localCursor -= clickRect.rect.position;
                float val = (localCursor.x / (m_HandleContainerRect.rect.width)) * (m_MaxValue - m_MinValue) +
                            m_MinValue;
                
                Set(val);
            }
        }

        private bool MayDrag(PointerEventData eventData)
        {
            return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;

            base.OnPointerDown(eventData);

            m_Offset = Vector2.zero;
            if (m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
            {
                Vector2 localMousePos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out localMousePos))
                    m_Offset = localMousePos;
            }
            else
            {
                // Outside the slider handle - jump to this point instead
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
                return;
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
            {
                base.OnMove(eventData);
                return;
            }

            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (axis == Axis.Horizontal && FindSelectableOnLeft() == null)
                        Set(reverseValue ? value + stepSize : value - stepSize);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    if (axis == Axis.Horizontal && FindSelectableOnRight() == null)
                        Set(reverseValue ? value - stepSize : value + stepSize);
                    else
                        base.OnMove(eventData);
                    break;
            }
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        public override void Enable(bool onOff, bool force = false)
        {
            UpdateVisuals();
            base.Enable(onOff);
        }

    }
}