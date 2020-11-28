using System;
using UnityEngine;
using UnityEngine.UI;

namespace HereBeElements.Shaders
{
    [Serializable]
    class CanvasRendererWrapper: IRenderer
    {
        [SerializeField]
        private Graphic _graphic;

        [SerializeField] private CanvasRenderer _r;

        public CanvasRendererWrapper(Graphic g, CanvasRenderer r)
        {
            _graphic = g;
            _r = r;
        }

        public bool CreateNewMaterialInstance()
        {
            if (_graphic.material == null)
                return false;
            try
            {
                _graphic.material = new Material(_graphic.material);
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
            if (_graphic != null)
                _graphic.material.SetInt(key, value);
        }

        public void SetColor(int key, Color color)
        {
            if (_graphic != null)
                _graphic.color = color;
        }

        public void SetFloat(int key, float value)
        {
            if (_graphic != null)
                _graphic.material.SetFloat(key, value);
        }

        public void SetAlpha(float value)
        {
            _r.SetAlpha(value);
        }
    }
}