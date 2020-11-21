using System;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace com.herebedragons.herebeelements.Runtime.Templates
{
    public interface ILoadable
    {
        IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null);
    }
}