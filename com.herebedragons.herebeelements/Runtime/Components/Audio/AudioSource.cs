using System;
using UnityEngine;

namespace HereBeElements.Audio
{
    [Serializable]
    public class AudioSource<T> where T : Enum
    {
        public T action;
        public AudioType audioType;
        public AudioClip audioClip;
    }
}