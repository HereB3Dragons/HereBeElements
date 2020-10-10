using System;
using com.herebedragons.herebeelements.Runtime.Templates;
using UnityEngine;
using UnityEngine.UI;

namespace HereBeElements.Components
{
    [Serializable]
    public class ShaderControl : MonoBehaviour
    {
        public bool isStatic = true;
        public ShaderConfig config = new ShaderConfig();

        public void ApplyConfig(Renderer r) => config.ApplyConfig(r);

        public void ApplyConfig(Graphic g) => config.ApplyConfig(g);

        private UIElement _owner;

        protected virtual void Awake()
        {
            _owner = GetComponent<UIElement>();
        }

        public float Opacity
        {
            get => config.opacity;
            set
            {
                config.opacity = value;
                config.SetFloat(ShaderConfig.Opacity, value);
            }
        }

        public float AlphaClip
        {
            get => config.alphaClipThreshold;
            set
            {
                config.opacity = value;
                config.SetFloat(ShaderConfig.AlphaClip, value);
            }
        }
        public Color TintColor
        {
            get => config.tintColor;
            set
            {
                config.tintColor = value;
                config.SetColor(ShaderConfig.TintColor, value);
            }
        }

        public bool Border
        {
            get => config.border;
            set
            {
                config.border = value;
                config.SetBool(ShaderConfig.Border, value);
            }
        }

        public float BorderWidth
        {
            get => config.borderWidth;
            set
            {
                config.borderWidth = value;
                config.SetFloat(ShaderConfig.BorderWidth, value);
            }
        }

        public Color BorderColor
        {
            get => config.borderColor;
            set
            {
                config.borderColor = value;
                config.SetColor(ShaderConfig.BorderColor, value);
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_owner == null) _owner = GetComponent<UIElement>();
            if ( _owner != null && _owner.GetGraphic() != null)
                ApplyConfig(_owner.GetGraphic());
        }
#endif
    }
}