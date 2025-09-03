using System;
using Urd.Services;
using UnityEngine.Audio;

namespace Urd.Audio
{
    public class AudioMixerModel
    {
        public AudioMixerType Type => _audioMixerData.MixerType;

        public bool IsEnabled { get; private set; } = true;
        public AudioMixerGroup MixerGroup => _audioMixerData.Mixer;
        
        private AudioMixerData _audioMixerData;
        
        public event Action<bool> OnEnabledChanged;
        
        public AudioMixerModel(AudioMixerData audioMixerData)
        {
            _audioMixerData = audioMixerData;
        }

        public void SetEnable(bool enable)
        {
            IsEnabled = enable;
            OnEnabledChanged?.Invoke(IsEnabled);
        }
    }
}