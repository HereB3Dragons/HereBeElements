using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace HereBeElements.Locale
{
    [Serializable]
    public class AvailableLocale
    {
        public string lang;
        public AssetReference jsonFile;

        public AvailableLocale(string lang, AssetReference reference)
        {
            this.lang = lang;
            this.jsonFile = reference;
        }

        private sealed class LangJsonFileEqualityComparer : IEqualityComparer<AvailableLocale>
        {
            public bool Equals(AvailableLocale x, AvailableLocale y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.lang == y.lang && Equals(x.jsonFile, y.jsonFile);
            }

            public int GetHashCode(AvailableLocale obj)
            {
                unchecked
                {
                    return ((obj.lang != null ? obj.lang.GetHashCode() : 0) * 397) ^ (obj.jsonFile != null ? obj.jsonFile.GetHashCode() : 0);
                }
            }
        }

        public static IEqualityComparer<AvailableLocale> LangJsonFileComparer { get; } = new LangJsonFileEqualityComparer();
    }
}