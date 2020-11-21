

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.herebedragons.herebeelements.Runtime.Materials
{
    public static class MaterialFactory
    {
        private static AssetReference _elementMaterialRef = new AssetReference("f03dc0c0fd17d40468cf90de47a15a5b");

        private static AssetReference _uiElementMaterialRef  = new AssetReference("e19c1ed3498554d44a36e1963597c895");

        public static Material UIElementMaterial { get; private set; }
        public static Material ElementMaterial { get; private set; }

        
        private static IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            var loadProcess = assetRef.LoadAssetAsync<T>();
            if (percentageSetter != null)
                percentageSetter(loadProcess.PercentComplete);
            yield return loadProcess;
            setter(loadProcess.Result);
        }
    }
}