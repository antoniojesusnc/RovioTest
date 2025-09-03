using System;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;

namespace Urd.Services
{
    [Serializable]
    public class AudioMixerData
    {
        [field: SerializeField] public AudioMixerType MixerType { get; private set; }

        [field: SerializeField] public AudioMixerGroup Mixer { get; private set; }

        public AudioMixerData(AudioMixerType mixerType, AudioMixerGroup mixer)
        {
            MixerType = mixerType;
            Mixer = mixer;
        }
    }
}