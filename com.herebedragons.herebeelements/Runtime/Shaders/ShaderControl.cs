using System;
using HereBeElements.Templates;
using UnityEngine;

namespace HereBeElements.Shaders
{
    [Serializable]
    public class ShaderControl : MonoBehaviour
    {
        [SerializeField]
        public bool isStatic = true;
        
        [SerializeField]
        public ShaderConfig config = new ShaderConfig();
        
        private IElement _owner;
        
        private IRenderer _renderer;

        protected virtual void Awake()
        {
            Init();
        }

        public bool ApplyConfig()
        {
            return config.ApplyConfig(_renderer);
        }

        public bool CreateNewMaterialInstance()
        {
            if (_renderer != null)
                return _renderer.CreateNewMaterialInstance();
            return false;
        }
        
        public float Opacity
        {
            get => config.opacity;
            set
            {
                config.opacity = value;
                
                config.SetAlpha(_renderer, value);
            }
        }

        public float AlphaClip
        {
            get => config.alphaClipThreshold;
            set
            {
                config.opacity = value;
                config.SetFloat(_renderer,ShaderConfig.AlphaClip, value);
            }
        }
        public Color TintColor
        {
            get => config.tintColor;
            set
            {
                config.tintColor = value;
                config.SetColor(_renderer,ShaderConfig.TintColor, value);
            }
        }

        public bool Border
        {
            get => config.border;
            set
            {
                config.border = value;
                config.SetBool(_renderer,ShaderConfig.Border, value);
            }
        }

        public float BorderWidth
        {
            get => config.borderWidth;
            set
            {
                config.borderWidth = value;
                config.SetFloat(_renderer, ShaderConfig.BorderWidth, value);
            }
        }

        public Color BorderColor
        {
            get => config.borderColor;
            set
            {
                config.borderColor = value;
                config.SetColor(_renderer,ShaderConfig.BorderColor, value);
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            Init();
            if (_owner == null)
                return;

            config.isStatic = isStatic;
            config.ApplyConfig(_renderer);
        }
#endif

        private void Init()
        {
            if (_owner == null)
                _owner = GetComponent<IElement>();
            if (_renderer != null || _owner == null) return;
            Type t = _owner.GetType();
            
            if (t == typeof(UIElement) || t.IsSubclassOf(typeof(UIElement)))
                _renderer = new CanvasRendererWrapper(((UIElement)_owner).GetGraphic(), GetComponent<CanvasRenderer>());
            if (t == typeof(Element) || t.IsSubclassOf(typeof(Element)))
                _renderer = new RendererWrapper(((Element)_owner).GetGraphic());
        }
  
    }
}