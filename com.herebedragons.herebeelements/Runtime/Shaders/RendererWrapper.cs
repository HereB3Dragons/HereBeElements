using System;
using UnityEngine;

namespace HereBeElements.Shaders
{
    [Serializable]
    class RendererWrapper: IRenderer
    {
        [SerializeField]
        private SpriteRenderer _renderer;
        private MaterialPropertyBlock _props = new MaterialPropertyBlock();

        public RendererWrapper(SpriteRenderer r)
        {
            _renderer = r;
            _renderer.GetPropertyBlock(_props);
        }
        
        public bool CreateNewMaterialInstance()
        {
            if (_renderer.sharedMaterial == null)
                return false;
            try
            {
                _renderer.materials = new Material[1];
                _renderer.materials[1] = new Material(_renderer.sharedMaterial);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        public void SetInt(int key, int value)
        {
            _renderer.GetPropertyBlock(_props);
            _props.SetInt(key, value);
            _renderer.SetPropertyBlock(_props);
        }

        public void SetColor(int key, Color color)
        {
            _renderer.GetPropertyBlock(_props);
            _props.SetColor(key, color);
            _renderer.SetPropertyBlock(_props);
        }

        public void SetFloat(int key, float value)
        {
            _renderer.GetPropertyBlock(_props);
            _props.SetFloat(key, value);
            _renderer.SetPropertyBlock(_props);
        }

        public void SetAlpha(float value)
        {
            _renderer.GetPropertyBlock(_props);
            _props.SetFloat(ShaderConfig.Opacity, value);
            _renderer.SetPropertyBlock(_props);
        }
    }
}