using System;
using HereBeElements.Internal;
using HereBeElements.Templates;
using UnityEngine;
using UnityEngine.UI;

namespace HereBeElements
{
    [RequireComponent(typeof(Text))]
    [Serializable]
    public class UIDisplay: UIElement, IDisplay
    {
        protected Text _text;

        public string placeHolder = "";

        public string text;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponent<Text>();
        }

        public virtual void Show(string text)
        {
            if (_text != null)
            {
                if (text.Equals(Utils.EMPTY))
                    text = placeHolder;
                _text.text = text;
            }
        }

        public virtual void Show(long value)
        {
            Show(value.ToString());
        }

        public virtual void Clear()
        {
            Show(Utils.EMPTY);
        }
      
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_text != null)
                Show(text);
        }
#endif
    }
}