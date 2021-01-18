using System;
using HereBeElements.Util;
using UnityEngine;

namespace HereBeElements.Animations.World
{
    public class SimpleTranslationAnimation: SimpleTranslationAnimation<Transform>
    {
        protected override Vector3 GetDestinationPosition(Direction d)
        {
            Transform t = GetTransform();
            Vector3 pos = t.localPosition;
            int orientation = GetState() ? -1 : 1;
            float delta, width, height;
            Vector3 dimensions = Vector3.Scale(transform.localScale, GetComponent<Mesh>().bounds.size); 
            switch (d)
            {
                case Direction.RightToLeft:
                    width = dimensions.x;
                    delta = width * orientation * (1 - buffer);
                    return new Vector3(pos.x - delta, pos.y, pos.z);
                case Direction.LeftToRight:
                    width = dimensions.x;
                    delta = width * orientation *(1 - buffer);
                    return new Vector3(pos.x + delta, pos.y, pos.z);
                case Direction.TopToBottom:
                    height = dimensions.z;
                    delta = height * orientation * (1 - buffer);
                    return new Vector3(pos.x, pos.y - delta, pos.z);
                case Direction.BottomToTop:
                    height = dimensions.z;
                    delta = height * orientation * (1 - buffer);
                    return new Vector3(pos.x, pos.y + delta, pos.z);
                default:
                    throw new ArgumentException("impossible");
            }
        }
    }
}