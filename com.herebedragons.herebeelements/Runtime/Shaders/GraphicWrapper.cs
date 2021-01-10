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

        private Color _color = Color.white;

        public CanvasRendererWrapper(Graphic g, CanvasRenderer r)
        {
            _graphic = g;
            if (g != null && g.material != null)
                _color = g.material.color;
            _r = r;
            //_r.SetColor(Color.white);
        }

        public bool CreateNewMaterialInstance()
        {
            if (_graphic == null || _graphic.material == null)
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
            {
                _graphic.color = color;
                _color = color;
            }
        }

        public void SetFloat(int key, float value)
        {
            if (_graphic != null)
                _graphic.material.SetFloat(key, value);
        }

        public void SetAlpha(float value)
        {
            if (_graphic != null)
            {
                Color c = _color;
                c.a = value * c.a;
                _graphic.color = c;
            }
        }
    }
}