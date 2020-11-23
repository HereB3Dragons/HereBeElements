using System;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace HereBeElements.Templates
{
    public interface ILoadable
    {
        IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null);

        void LoadAsset<T>(AssetReference assetRef, Action<T> setter);
    }
}