using System;
using UnityEngine;

namespace Urd.Audio
{
    public interface IAudioConfigData 
    {
        Enum Type { get; }
        AudioClip Clip { get; }
        float Volume { get; }
        AudioMixerType Mixer { get;  }
        float Pitch { get; }
        public bool Loop { get; }
        float FadeOutDuration { get; }
    }
}
