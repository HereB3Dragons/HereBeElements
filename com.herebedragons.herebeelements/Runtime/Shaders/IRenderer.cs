using UnityEngine;

namespace HereBeElements.Shaders
{
    public interface IRenderer
    {
        void SetInt(int key, int value);

        void SetColor(int key, Color color);

        void SetFloat(int key, float value);

        void SetAlpha(float value);

        bool CreateNewMaterialInstance();
    }
}