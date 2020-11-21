using System;
using System.Collections;
using UnityEngine.AddressableAssets;

namespace Internal
{
    public static class Utils
    {
        public static IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            var loadProcess = assetRef.LoadAssetAsync<T>();
            if (percentageSetter != null)
                percentageSetter(loadProcess.PercentComplete);
            yield return loadProcess;
            setter(loadProcess.Result);
        }
    }
}