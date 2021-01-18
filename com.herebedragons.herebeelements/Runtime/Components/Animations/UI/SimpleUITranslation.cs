using System;
using HereBeElements.Util;
using UnityEngine;

namespace HereBeElements.Animations.UI
{
    public class SimpleUITranslation: SimpleTranslationAnimation<RectTransform>
    {
        protected override Vector3 GetDestinationPosition(Direction d)
        {
            RectTransform t = GetTransform();
            Vector3 pos = t.localPosition;
            int orientation = GetState() ? -1 : 1;
            float delta, width, height;
            
            switch (d)
            {
                case Direction.RightToLeft:
                    width = t.rect.width;
                    delta = width * orientation * (1 - buffer);
                    return new Vector3(pos.x - delta, pos.y, pos.z);
                case Direction.LeftToRight:
                    width = t.rect.width;
                    delta = width * orientation *(1 - buffer);
                    return new Vector3(pos.x + delta, pos.y, pos.z);
                case Direction.TopToBottom:
                    height = t.rect.height;
                    delta = height * orientation * (1 - buffer);
                    return new Vector3(pos.x, pos.y - delta, pos.z);
                case Direction.BottomToTop:
                    height = t.rect.height;
                    delta = height * orientation * (1 - buffer);
                    return new Vector3(pos.x, pos.y + delta, pos.z);
                default:
                    throw new ArgumentException("impossible");
            }
        }
    }
}