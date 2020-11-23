using System;
using HereBeElements.Templates;
using UnityEngine;
using UnityEngine.UI;

namespace HereBeElements.Components
{
    [Serializable]
    public class ShaderControl : MonoBehaviour
    {
        [SerializeField]
        public bool isStatic = true;

        private bool _isStatic = true;

        public ShaderConfig config = new ShaderConfig();

        public void ApplyConfig(Renderer r) => config.ApplyConfig(r);

        public void ApplyConfig(Graphic g) => config.ApplyConfig(g);

        private IElement _owner;

        protected virtual void Awake()
        {
            _owner = GetComponent<IElement>();
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
            _owner = GetComponent<IElement>();
            if (_owner == null)
                return;

            if (_owner.GetType() == typeof(UIElement))
            {
                Graphic el = ((UIElement) _owner).GetGraphic();
                if (el is null) return;
                if (!isStatic && _isStatic)
                    config.DuplicateMaterial(el);
                
                ApplyConfig(el);
            }
            
            if (_owner.GetType() == typeof(Element))
            {
                Renderer el = ((Element) _owner).GetGraphic();
                if (el is null) return;
                
                if (!isStatic && _isStatic)
                    config.DuplicateMaterial(el);
                
                ApplyConfig(el);
            }

            _isStatic = isStatic;
        }
#endif
    }
}