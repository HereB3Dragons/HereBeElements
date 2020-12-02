using System;
using System.Collections;
using HereBeElements.Internal;
using HereBeElements.Shaders;
using HereBeElements.Templates;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


namespace HereBeElements.UI
{
    [RequireComponent(typeof(ShaderControl))]
    public class UIImage : Image, ILoadable
    {
        [NonSerialized]
        private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

        public AssetReference imageAssetReference;
        
        protected ShaderControl _sc;

        protected UIImage()
        {
            if (m_ColorTweenRunner == null)
                m_ColorTweenRunner = new TweenRunner<ColorTween>();
            m_ColorTweenRunner.Init(this);
        }

        protected override void Awake()
        {
            base.Awake();
            _sc = GetComponent<ShaderControl>();
            if (imageAssetReference != null)
                LoadContent<Material>(imageAssetReference, res => this.material = res);
        }

        public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
        {
            if (_sc == null || (!useRGB && !useAlpha))
                return;

            Color currentColor = _sc.TintColor;
            if (currentColor.Equals(targetColor))
            {
                m_ColorTweenRunner.StopTween();
                return;
            }

            ColorTween.ColorTweenMode mode = (useRGB && useAlpha ?
                ColorTween.ColorTweenMode.All :
                (useRGB ? ColorTween.ColorTweenMode.RGB : ColorTween.ColorTweenMode.Alpha));

            var colorTween = new ColorTween {duration = duration, startColor = _sc.TintColor, targetColor = targetColor};
            colorTween.AddOnChangedCallback((c) => _sc.TintColor = c);
            colorTween.ignoreTimeScale = ignoreTimeScale;
            colorTween.tweenMode = mode;
            m_ColorTweenRunner.StartTween(colorTween);
        }

        public IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            return Utils.LoadContent(assetRef, setter, percentageSetter);
        }
        
        public void LoadAsset<T>(AssetReference assetRef, Action<T> setter)
        {
            Utils.LoadAsset(assetRef, setter);
        }
    }
}