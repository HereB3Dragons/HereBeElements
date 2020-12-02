using System;
using System.Collections;
using HereBeElements.Shaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HereBeElements.Internal
{
    [RequireComponent(typeof(ShaderControl))]
    public class InGameSelectable : MonoBehaviour, IMoveHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISelectable
    {
        
        protected static InGameSelectable[] s_Selectables = new InGameSelectable[10];
        protected static int s_SelectableCount = 0;
        private bool m_EnableCalled = false;

        protected ShaderControl _sc;

        public static InGameSelectable[] allSelectablesArray
        {
            get
            {
                InGameSelectable[] temp = new InGameSelectable[s_SelectableCount];
                Array.Copy(s_Selectables, temp, s_SelectableCount);
                return temp;
            }
        }

        public bool IsActive()
        {
            throw new NotImplementedException();
        }

        public static int AllSelectablesNoAlloc(InGameSelectable[] selectables)
        {
            int copyCount = selectables.Length < s_SelectableCount ? selectables.Length : s_SelectableCount;

            Array.Copy(s_Selectables, selectables, copyCount);

            return copyCount;
        }
        
         // Type of the transition that occurs when the button state changes.
        [SerializeField]
        private Selectable.Transition m_Transition = Selectable.Transition.ColorTint;

        // Colors used for a color tint-based transition.
        [SerializeField]
        private UIColorBlock m_Colors = UIColorBlock.defaultColorBlock;

        // Sprites used for a Image swap-based transition.
        [SerializeField]
        private SpriteState m_SpriteState;

        [SerializeField]
        private AnimationTriggers m_AnimationTriggers = new AnimationTriggers();

        [Tooltip("Can the Selectable be interacted with?")]
        [SerializeField]
        private bool m_Interactable = true;

        // Graphic that will be colored.
        [SerializeField]
        private SpriteRenderer m_TargetGraphic;


        private bool m_GroupsAllowInteraction = true;
        protected int m_CurrentIndex = -1;
        
        private readonly TweenRunner<ColorTween> m_ColorTweenRunner;
        
        [FormerlySerializedAs("navigation")]
        [SerializeField]
        private InGameNavigation m_Navigation = InGameNavigation.defaultNavigation;
        public InGameNavigation        navigation        { get { return m_Navigation; } set { if (SetPropertyUtility.SetStruct(ref m_Navigation, value))        OnSetProperty(); } }

       
        public Selectable.Transition        transition        { get { return m_Transition; } set { if (SetPropertyUtility.SetStruct(ref m_Transition, value))        OnSetProperty(); } }

       
        public UIColorBlock        colors            { get { return m_Colors; } set { if (SetPropertyUtility.SetStruct(ref m_Colors, value))            OnSetProperty(); } }

       
        public SpriteState       spriteState       { get { return m_SpriteState; } set { if (SetPropertyUtility.SetStruct(ref m_SpriteState, value))       OnSetProperty(); } }

      
        public AnimationTriggers animationTriggers { get { return m_AnimationTriggers; } set { if (SetPropertyUtility.SetClass(ref m_AnimationTriggers, value)) OnSetProperty(); } }

      
        public SpriteRenderer           targetGraphic     { get { return m_TargetGraphic; } set { if (SetPropertyUtility.SetClass(ref m_TargetGraphic, value))     OnSetProperty(); } }

      
        public bool              interactable
        {
            get { return m_Interactable; }
            set
            {
                if (SetPropertyUtility.SetStruct(ref m_Interactable, value))
                {
                    if (!m_Interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                        EventSystem.current.SetSelectedGameObject(null);
                    OnSetProperty();
                }
            }
        }

        private bool             isPointerInside   { get; set; }
        private bool             isPointerDown     { get; set; }
        private bool             hasSelection      { get; set; }

        protected InGameSelectable()
        {
            if (m_ColorTweenRunner == null)
                m_ColorTweenRunner = new TweenRunner<ColorTween>();
            m_ColorTweenRunner.Init(this);
        }
        
#if PACKAGE_ANIMATION
        public Animator animator
        {
            get { return GetComponent<Animator>(); }
        }
#endif

        protected virtual void Awake()
        {
            _sc = GetComponent<ShaderControl>(); 
            m_TargetGraphic = GetComponent<SpriteRenderer>();
        }
        
      public virtual bool IsInteractable()
        {
            return m_GroupsAllowInteraction && m_Interactable;
        }

        // Call from unity if animation properties have changed
        protected virtual void OnDidApplyAnimationProperties()
        {
            OnSetProperty();
        }

        // Select on enable and add to the list.
        protected virtual void OnEnable()
        {
            //Check to avoid multiple OnEnable() calls for each selectable
            if (m_EnableCalled)
                return;

            //base.OnEnable();

            if (s_SelectableCount == s_Selectables.Length)
            {
                InGameSelectable[] temp = new InGameSelectable[s_Selectables.Length * 2];
                Array.Copy(s_Selectables, temp, s_Selectables.Length);
                s_Selectables = temp;
            }
            m_CurrentIndex = s_SelectableCount;
            s_Selectables[m_CurrentIndex] = this;
            s_SelectableCount++;
            isPointerDown = false;
            DoStateTransition(currentSelectionState, true);

            m_EnableCalled = true;
        }

        private void OnSetProperty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DoStateTransition(currentSelectionState, true);
            else
#endif
            DoStateTransition(currentSelectionState, false);
        }

        // Remove from the list.
        protected virtual void OnDisable()
        {
            //Check to avoid multiple OnDisable() calls for each selectable
            if (!m_EnableCalled)
                return;

            s_SelectableCount--;

            // Update the last elements index to be this index
            s_Selectables[s_SelectableCount].m_CurrentIndex = m_CurrentIndex;

            // Swap the last element and this element
            s_Selectables[m_CurrentIndex] = s_Selectables[s_SelectableCount];

            // null out last element.
            s_Selectables[s_SelectableCount] = null;

            InstantClearState();

            m_EnableCalled = false;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            //base.OnValidate();
            if (_sc == null)
                _sc = GetComponent<ShaderControl>();
            m_Colors.fadeDuration = Mathf.Max(m_Colors.fadeDuration, 0.0f);

            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            // OnSetProperty potentially access Animator or Graphics. (case 618186)
            if (isActiveAndEnabled)
            {
                if (!interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                    EventSystem.current.SetSelectedGameObject(null);
                // Need to clear out the override image on the target...
                //DoSpriteSwap(null);

                // If the transition mode got changed, we need to clear all the transitions, since we don't know what the old transition mode was.
                StartColorTween(Color.white, true);
                TriggerAnimation(m_AnimationTriggers.normalTrigger);

                // And now go to the right state.
                DoStateTransition(currentSelectionState, true);
            }
        }

        protected virtual void Reset()
        {
            m_TargetGraphic = GetComponent<SpriteRenderer>();
        }

#endif // if UNITY_EDITOR

        protected virtual SelectionState currentSelectionState
        {
            get
            {
                if (!IsInteractable())
                    return SelectionState.Disabled;
                if (isPointerDown)
                    return SelectionState.Pressed;
                if (hasSelection)
                    return SelectionState.Selected;
                if (isPointerInside)
                    return SelectionState.Highlighted;
                return SelectionState.Normal;
            }
        }

        /// <summary>
        /// Clear any internal state from the Selectable (used when disabling).
        /// </summary>
        protected virtual void InstantClearState()
        {
            string triggerName = m_AnimationTriggers.normalTrigger;

            isPointerInside = false;
            isPointerDown = false;
            hasSelection = false;

            switch (m_Transition)
            {
                case Selectable.Transition.ColorTint:
                    StartColorTween(Color.white, true);
                    break;
                case Selectable.Transition.SpriteSwap:
                    DoSpriteSwap(null);
                    break;
                case Selectable.Transition.Animation:
                    TriggerAnimation(triggerName);
                    break;
            }
        }

        /// <summary>
        /// Transition the Selectable to the entered state.
        /// </summary>
        /// <param name="state">State to transition to</param>
        /// <param name="instant">Should the transition occur instantly.</param>
        protected virtual void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            Color tintColor;
            Sprite transitionSprite;
            string triggerName;

            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = m_Colors.normalColor;
                    transitionSprite = null;
                    triggerName = m_AnimationTriggers.normalTrigger;
                    break;
                case SelectionState.Highlighted:
                    tintColor = m_Colors.highlightedColor;
                    transitionSprite = m_SpriteState.highlightedSprite;
                    triggerName = m_AnimationTriggers.highlightedTrigger;
                    break;
                case SelectionState.Pressed:
                    tintColor = m_Colors.pressedColor;
                    transitionSprite = m_SpriteState.pressedSprite;
                    triggerName = m_AnimationTriggers.pressedTrigger;
                    break;
                case SelectionState.Selected:
                    tintColor = m_Colors.selectedColor;
                    transitionSprite = m_SpriteState.selectedSprite;
                    triggerName = m_AnimationTriggers.selectedTrigger;
                    break;
                case SelectionState.Disabled:
                    tintColor = m_Colors.disabledColor;
                    transitionSprite = m_SpriteState.disabledSprite;
                    triggerName = m_AnimationTriggers.disabledTrigger;
                    break;
                default:
                    tintColor = Color.black;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }

            switch (m_Transition)
            {
                case Selectable.Transition.ColorTint:
                    StartColorTween(tintColor * m_Colors.colorMultiplier, instant);
                    break;
                case Selectable.Transition.SpriteSwap:
                    DoSpriteSwap(transitionSprite);
                    break;
                case Selectable.Transition.Animation:
                    TriggerAnimation(triggerName);
                    break;
            }
        }

        /// <summary>
        /// An enumeration of selected states of objects
        /// </summary>
        protected enum SelectionState
        {
            /// <summary>
            /// The UI object can be selected.
            /// </summary>
            Normal,

            /// <summary>
            /// The UI object is highlighted.
            /// </summary>
            Highlighted,

            /// <summary>
            /// The UI object is pressed.
            /// </summary>
            Pressed,

            /// <summary>
            /// The UI object is selected
            /// </summary>
            Selected,

            /// <summary>
            /// The UI object cannot be selected.
            /// </summary>
            Disabled,
        }

        // Selection logic

        /// <summary>
        /// Finds the selectable object next to this one.
        /// </summary>
        /// <remarks>
        /// The direction is determined by a Vector3 variable.
        /// </remarks>
        /// <param name="dir">The direction in which to search for a neighbouring Selectable object.</param>
        /// <returns>The neighbouring Selectable object. Null if none found.</returns>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // required when using UI elements in scripts
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     //Sets the direction as "Up" (Y is in positive).
        ///     public Vector3 direction = new Vector3(0, 1, 0);
        ///     public Button btnMain;
        ///
        ///     public void Start()
        ///     {
        ///         //Finds and assigns the selectable above the main button
        ///         Selectable newSelectable = btnMain.FindSelectable(direction);
        ///
        ///         Debug.Log(newSelectable.name);
        ///     }
        /// }
        /// </code>
        /// </example>
        public InGameSelectable FindSelectable(Vector3 dir)
        {
            dir = dir.normalized;
            Vector3 localDir = Quaternion.Inverse(transform.rotation) * dir;
            Vector3 pos = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, localDir));
            float maxScore = Mathf.NegativeInfinity;
            InGameSelectable bestPick = null;

            for (int i = 0; i < s_SelectableCount; ++i)
            {
                InGameSelectable sel = s_Selectables[i];

                if (sel == this)
                    continue;

                if (!sel.IsInteractable() || sel.navigation.mode == Navigation.Mode.None)
                    continue;

#if UNITY_EDITOR
                // Apart from runtime use, FindSelectable is used by custom editors to
                // draw arrows between different selectables. For scene view cameras,
                // only selectables in the same stage should be considered.
                if (Camera.current != null && !UnityEditor.SceneManagement.StageUtility.IsGameObjectRenderedByCamera(sel.gameObject, Camera.current))
                    continue;
#endif

                var selRect = sel.transform as RectTransform;
                Vector3 selCenter = selRect != null ? (Vector3)selRect.rect.center : Vector3.zero;
                Vector3 myVector = sel.transform.TransformPoint(selCenter) - pos;

                // Value that is the distance out along the direction.
                float dot = Vector3.Dot(dir, myVector);

                // Skip elements that are in the wrong direction or which have zero distance.
                // This also ensures that the scoring formula below will not have a division by zero error.
                if (dot <= 0)
                    continue;

                // This scoring function has two priorities:
                // - Score higher for positions that are closer.
                // - Score higher for positions that are located in the right direction.
                // This scoring function combines both of these criteria.
                // It can be seen as this:
                //   Dot (dir, myVector.normalized) / myVector.magnitude
                // The first part equals 1 if the direction of myVector is the same as dir, and 0 if it's orthogonal.
                // The second part scores lower the greater the distance is by dividing by the distance.
                // The formula below is equivalent but more optimized.
                //
                // If a given score is chosen, the positions that evaluate to that score will form a circle
                // that touches pos and whose center is located along dir. A way to visualize the resulting functionality is this:
                // From the position pos, blow up a circular balloon so it grows in the direction of dir.
                // The first Selectable whose center the circular balloon touches is the one that's chosen.
                float score = dot / myVector.sqrMagnitude;

                if (score > maxScore)
                {
                    maxScore = score;
                    bestPick = sel;
                }
            }
            return bestPick;
        }

        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;
            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

        // Convenience function -- change the selection to the specified object if it's not null and happens to be active.
        void Navigate(AxisEventData eventData, ISelectable sel)
        {
            if (sel != null /*&& sel.IsActive()*/)
                eventData.selectedObject = sel.GetGameObject();
        }

        /// <summary>
        /// Find the selectable object to the left of this one.
        /// </summary>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // required when using UI elements in scripts
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     public Button btnMain;
        ///
        ///     // Disables the selectable UI element directly to the left of the Start Button
        ///     public void IgnoreSelectables()
        ///     {
        ///         //Finds the selectable UI element to the left the start button and assigns it to a variable of type "Selectable"
        ///         Selectable secondButton = startButton.FindSelectableOnLeft();
        ///         //Disables interaction with the selectable UI element
        ///         secondButton.interactable = false;
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual ISelectable FindSelectableOnLeft()
        {
            if (m_Navigation.mode == Navigation.Mode.Explicit)
            {
                return m_Navigation.selectOnLeft;
            }
            if ((m_Navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.left);
            }
            return null;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        /// <summary>
        /// Find the selectable object to the right of this one.
        /// </summary>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // required when using UI elements in scripts
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     public Button btnMain;
        ///
        ///     // Disables the selectable UI element directly to the right the Start Button
        ///     public void IgnoreSelectables()
        ///     {
        ///         //Finds the selectable UI element to the right the start button and assigns it to a variable of type "Selectable"
        ///         Selectable secondButton = startButton.FindSelectableOnRight();
        ///         //Disables interaction with the selectable UI element
        ///         secondButton.interactable = false;
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual ISelectable FindSelectableOnRight()
        {
            if (m_Navigation.mode == Navigation.Mode.Explicit)
            {
                return m_Navigation.selectOnRight;
            }
            if ((m_Navigation.mode & Navigation.Mode.Horizontal) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.right);
            }
            return null;
        }

        /// <summary>
        /// The Selectable object above current
        /// </summary>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // required when using UI elements in scripts
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     public Button btnMain;
        ///
        ///     // Disables the selectable UI element directly above the Start Button
        ///     public void IgnoreSelectables()
        ///     {
        ///         //Finds the selectable UI element above the start button and assigns it to a variable of type "Selectable"
        ///         Selectable secondButton = startButton.FindSelectableOnUp();
        ///         //Disables interaction with the selectable UI element
        ///         secondButton.interactable = false;
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual ISelectable FindSelectableOnUp()
        {
            if (m_Navigation.mode == Navigation.Mode.Explicit)
            {
                return m_Navigation.selectOnUp;
            }
            if ((m_Navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.up);
            }
            return null;
        }

        /// <summary>
        /// Find the selectable object below this one.
        /// </summary>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // required when using UI elements in scripts
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     public Button startButton;
        ///
        ///     // Disables the selectable UI element directly below the Start Button
        ///     public void IgnoreSelectables()
        ///     {
        ///         //Finds the selectable UI element below the start button and assigns it to a variable of type "Selectable"
        ///         Selectable secondButton = startButton.FindSelectableOnDown();
        ///         //Disables interaction with the selectable UI element
        ///         secondButton.interactable = false;
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual ISelectable FindSelectableOnDown()
        {
            if (m_Navigation.mode == Navigation.Mode.Explicit)
            {
                return m_Navigation.selectOnDown;
            }
            if ((m_Navigation.mode & Navigation.Mode.Vertical) != 0)
            {
                return FindSelectable(transform.rotation * Vector3.down);
            }
            return null;
        }

        /// <summary>
        /// Determine in which of the 4 move directions the next selectable object should be found.
        /// </summary>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI;
        /// using UnityEngine.EventSystems;// Required when using Event data.
        ///
        /// public class ExampleClass : MonoBehaviour, IMoveHandler
        /// {
        ///     //When the focus moves to another selectable object, Invoke this Method.
        ///     public void OnMove(AxisEventData eventData)
        ///     {
        ///         //Assigns the move direction and the raw input vector representing the direction from the event data.
        ///         MoveDirection moveDir = eventData.moveDir;
        ///         Vector2 moveVector = eventData.moveVector;
        ///
        ///         //Displays the information in the console
        ///         Debug.Log(moveDir + ", " + moveVector);
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    Navigate(eventData, FindSelectableOnRight());
                    break;

                case MoveDirection.Up:
                    Navigate(eventData, FindSelectableOnUp());
                    break;

                case MoveDirection.Left:
                    Navigate(eventData, FindSelectableOnLeft());
                    break;

                case MoveDirection.Down:
                    Navigate(eventData, FindSelectableOnDown());
                    break;
            }
        }

        void StartColorTween(Color targetColor, bool instant)
        {
            if (m_TargetGraphic == null)
                return;

            CrossFadeColor(targetColor, instant ? 0f : m_Colors.fadeDuration, true, true, true);
        }
        
        public virtual void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
        {
            if (m_TargetGraphic == null || _sc == null || (!useRGB && !useAlpha))
                return;

            Color currentColor = _sc.TintColor;
            if (currentColor.Equals(targetColor))
            {
                m_ColorTweenRunner.StopTween();
                return;
            }

            ColorTween.ColorTweenMode mode = (useRGB && useAlpha ?
                ColorTween.ColorTweenMode.All :
                (useRGB ? ColorTween.ColorTweenMode.RGB : ColorTween.ColorTweenMode.Alpha));

            var colorTween = new ColorTween {duration = duration, startColor = _sc.TintColor, targetColor = targetColor};
            colorTween.AddOnChangedCallback((c) => _sc.TintColor = c);
            colorTween.ignoreTimeScale = ignoreTimeScale;
            colorTween.tweenMode = mode;
            m_ColorTweenRunner.StartTween(colorTween);
        }

        void DoSpriteSwap(Sprite newSprite)
        {
            if (m_TargetGraphic == null)
                return;
            
            m_TargetGraphic.sprite = newSprite;
        }

        void TriggerAnimation(string triggername)
        {
#if PACKAGE_ANIMATION
            if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
            animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.selectedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);

            animator.SetTrigger(triggername);
#endif
        }

       
        protected bool IsHighlighted()
        {
            if (!IsInteractable())
                return false;
            return isPointerInside && !isPointerDown && !hasSelection;
        }

        /// <summary>
        /// Whether the current selectable is being pressed.
        /// </summary>
        protected bool IsPressed()
        {
            if (!IsInteractable())
                return false;
            return isPointerDown;
        }

        // Change the button to the correct state
        private void EvaluateAndTransitionToSelectionState()
        {
            if (!IsInteractable())
                return;

            DoStateTransition(currentSelectionState, false);
        }

     
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            // Selection tracking
            if (IsInteractable() && navigation.mode != Navigation.Mode.None && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            isPointerDown = true;
            EvaluateAndTransitionToSelectionState();
        }

      
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            isPointerDown = false;
            EvaluateAndTransitionToSelectionState();
        }

        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
            EvaluateAndTransitionToSelectionState();
        }

     
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
            EvaluateAndTransitionToSelectionState();
        }

    
        public virtual void OnSelect(BaseEventData eventData)
        {
            hasSelection = true;
            EvaluateAndTransitionToSelectionState();
        }

      
        public virtual void OnDeselect(BaseEventData eventData)
        {
            hasSelection = false;
            EvaluateAndTransitionToSelectionState();
        }

     
        public virtual void Select()
        {
            if (EventSystem.current == null || EventSystem.current.alreadySelecting)
                return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            return Utils.LoadContent(assetRef, setter, percentageSetter);
        }
        
        public void LoadAsset<T>(AssetReference assetRef, Action<T> setter)
        {
            Utils.LoadAsset(assetRef, setter);
        }
    }
}