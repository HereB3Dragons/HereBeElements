using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HereBeElements.Audio
{
    [Serializable]
    public abstract class AudioLibrary<T>: MonoBehaviour where T: Enum
    {
        protected abstract AudioSource<T>[] GetAudioSources();
        public virtual Dictionary<T, AudioSource<T>[]> GetSources()
        {
            if (GetAudioSources() == null) return null;
            return GetAudioSources().GroupBy(s => s.action)
                .ToDictionary(s => s.Key,
                    s => s.ToArray());
        }
    }
}