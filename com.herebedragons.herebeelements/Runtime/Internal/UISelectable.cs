using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace HereBeElements.Internal
{
    [Serializable]
    public class UISelectable : Selectable
    {
        
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            Color tintColor;
            Sprite transitionSprite;
            string triggerName;

            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = colors.normalColor;
                    transitionSprite = null;
                    triggerName = animationTriggers.normalTrigger;
                    break;
                case SelectionState.Highlighted:
                    tintColor = colors.highlightedColor;
                    transitionSprite = spriteState.highlightedSprite;
                    triggerName = animationTriggers.highlightedTrigger;
                    break;
                case SelectionState.Pressed:
                    tintColor = colors.pressedColor;
                    transitionSprite = spriteState.pressedSprite;
                    triggerName = animationTriggers.pressedTrigger;
                    break;
                case SelectionState.Selected:
                    tintColor = colors.selectedColor;
                    transitionSprite = spriteState.selectedSprite;
                    triggerName = animationTriggers.selectedTrigger;
                    break;
                case SelectionState.Disabled:
                    tintColor = colors.disabledColor;
                    transitionSprite = spriteState.disabledSprite;
                    triggerName = animationTriggers.disabledTrigger;
                    break;
                default:
                    tintColor = Color.black;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }
            
            
            switch (transition)
            {
                case Transition.ColorTint:
                    StartColorTween(tintColor * colors.colorMultiplier, instant);
                    break;
                case Transition.SpriteSwap:
                    DoSpriteSwap(transitionSprite);
                    break;
                case Transition.Animation:
                    TriggerAnimation(triggerName);
                    break;
            }

        }
        
        protected new SelectionState currentSelectionState
        {
            get
            {
                if (!IsInteractable())
                    return SelectionState.Disabled;
                //if (isPointerInside)
                //    return SelectionState.Highlighted;
                SelectionState s = base.currentSelectionState;
                return SelectionState.Highlighted == s ? SelectionState.Normal : s;
            }
        }
        
        void StartColorTween(Color targetColor, bool instant)
        {
            if (targetGraphic == null)
                return;

            targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        void DoSpriteSwap(Sprite newSprite)
        {
            if (image == null)
                return;

            image.overrideSprite = newSprite;
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
        [SerializeField]
        private UIColorBlock mm_Colors = UIColorBlock.defaultColorBlock;

        private void UpdateInternalColorBlock()
        {
            colors = UIColorBlock.ToColorBlock(mm_Colors);
        } 
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            UpdateInternalColorBlock();
            base.OnValidate();
        }

        protected override void Reset()
        {
            targetGraphic = GetComponent<Graphic>();
        }
#endif
        private void OnSetProperty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DoStateTransition(currentSelectionState, true);
            else
#endif
                DoStateTransition(currentSelectionState, false);
        }
        
        public virtual IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            return Utils.LoadContent(assetRef, setter, percentageSetter);
        }
        
        public virtual void LoadAsset<T>(AssetReference assetRef, Action<T> setter)
        {
            Utils.LoadAsset(assetRef, setter);
        }
    }
}