using System;
using HereBeElements.Components;
using Internal;
using UnityEngine;
using UnityEngine.UI;

namespace com.herebedragons.herebeelements.Runtime.Templates
{
    [RequireComponent(typeof(ShaderControl))]
    public class UIImage : Image
    {
        [NonSerialized]
        private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

        private ShaderControl _sc;

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
    }
}