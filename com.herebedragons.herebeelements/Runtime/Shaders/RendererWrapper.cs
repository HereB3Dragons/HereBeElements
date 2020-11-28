using System;
using UnityEngine;

namespace HereBeElements.Shaders
{
    [Serializable]
    class RendererWrapper: IRenderer
    {
        [SerializeField]
        private SpriteRenderer _renderer;
        [SerializeField]
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
            if (_renderer.material != null)
                _renderer.material.SetInt(key, value);
        }

        public void SetColor(int key, Color color)
        {
            _renderer.color = color;
        }

        public void SetFloat(int key, float value)
        {
            if (_renderer.material != null)
                _renderer.material.SetFloat(key, value);
        }

        public void SetAlpha(float value)
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(props);
            props.SetFloat(ShaderConfig.Opacity, value);
            _renderer.SetPropertyBlock(props);
        }
    }
}