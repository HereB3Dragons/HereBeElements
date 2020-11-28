using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace HereBeElements.Shaders
{
    [Serializable]
    public class ShaderConfig
    {
        [Header("General")] 
        [Range(0, 1)]
        [Tooltip("Sets the overall opacity")]
        public float opacity = 1;
        
        [ColorUsage(true, true)]
        [Tooltip("Sets the color tint")]
        public Color tintColor = Color.white;
        
        [Tooltip("Determines the 0 level opacity")]
        public float alphaClipThreshold = 0;
        [Tooltip("Turns Border On or Off")]
        public bool border = false;

        [Range(0, 0.99f)] public float borderWidth = 0.5f;
        [ColorUsage(true, true)]
        public Color borderColor = Color.white;

        private readonly Dictionary<int, Object> _cachedValues = new Dictionary<int, object>();

        public bool isStatic = true;

        // Base
        internal static readonly int Opacity = Shader.PropertyToID("opacity");
        internal static readonly int AlphaClip = Shader.PropertyToID("alphaClip");
        internal static readonly int TintColor = Shader.PropertyToID("_Color");

        // Border
        internal static readonly int Border = Shader.PropertyToID("border");
        internal static readonly int BorderWidth = Shader.PropertyToID("borderWidth");
        internal static readonly int BorderColor = Shader.PropertyToID("borderColor");
        
        [SerializeField]
        private Material material;
        
        

        void CloneMaterial(Renderer renderer)
        {
            material = new Material(renderer.material);
            renderer.material = material;
        }
        
        void CloneMaterial(Graphic renderer)
        {
            material = new Material(renderer.material);
            renderer.material = material;
        }
        
        internal bool ApplyConfig(IRenderer renderer)
        {
            try
            {
                if (isStatic || renderer == null) return false;
                
                SetAlpha(renderer, opacity);
                
                // general
                //SetFloat(renderer,Opacity, opacity);
                //SetFloat(renderer,AlphaClip, alphaClipThreshold);
                SetColor(renderer,TintColor, tintColor);

                // Bor
                // SetBool(renderer,Border, border);
                // SetFloat(renderer,BorderWidth, borderWidth);
                // SetColor(renderer,BorderColor, borderColor);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }
      
        public void SetBool(IRenderer renderer, int key, bool value)
        {
            if (CheckMaterial(renderer, key, value))
            {
                try
                {
                    renderer.SetInt(key, value ? 1 : 0);
                    _cachedValues[key] = value;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public void SetColor(IRenderer renderer, int key, Color color)
        {
            if (CheckMaterial(renderer, key, color))
            {
                try
                {
                    renderer.SetColor(key, new Color(color.r, color.g, color.b, color.a * opacity));
                    _cachedValues[key] = color;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public void SetAlpha(IRenderer renderer, float value)
        {
            if (CheckMaterial(renderer, Opacity, value))
            {
                try
                {
                    renderer.SetAlpha(value);
                    _cachedValues[Opacity] = value;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        
        public void SetFloat(IRenderer renderer, int key, float value)
        {
            if (CheckMaterial(renderer, key, value))
            {
                try
                {
                    renderer.SetFloat(key, value);
                    _cachedValues[key] = value;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        private bool CheckMaterial(IRenderer renderer, int key, Object value)
        {
            if (renderer == null)
                return false;
            if (_cachedValues.TryGetValue(key, out Object o))
                return !o.Equals(value);
            return true;
        }
    }
}