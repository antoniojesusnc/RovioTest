using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.Services;

namespace Urd.Audio
{
    public class AudioModel : IDisposable
    {
        public Enum AudioType { get; private set; }
        public Transform AudioLocation { get; private set; } = null;

        public float FadeOut => _fadeOut > 0
            ? _fadeOut
            : AudioData.FadeOutDuration;
        
        public float _fadeOut;
        private float _volume = int.MaxValue;

        public float Volume => _volume != int.MaxValue
            ? _volume
            : AudioData.Volume;
        
        private float _pitch = int.MaxValue;

        public float Pitch => _pitch != int.MaxValue
            ? _pitch
            : AudioData.Pitch;
        private AudioMixerType _audioMixerType = AudioMixerType.None;
        public AudioMixerType AudioMixerType => _audioMixerType != AudioMixerType.None 
            ? _audioMixerType
            : AudioData.Mixer ;
        public IAudioData AudioData { get; private set; }
        private AudioClip _clip;
        public AudioClip Clip => _clip != null
            ? _clip
            : AudioData.Clip;
        public List<AudioClip> AdditionalClips => AudioData.AdditionalClips;
        
        public bool Loop => AudioData.Loop;

        public AudioModel(Enum audioType)
        {
            AudioType = audioType;
        }
        
        public void SetAudioClip(AudioClip audioClip)
        {
            _clip = audioClip;
        }

        public void SetAudioLocation(Transform audioLocation)
        {
            AudioLocation = audioLocation;
        }
        
        public void SetVolume(float volume)
        {
            _volume = volume;
        }
        
        public void SetToDefaultVolume()
        {
            _volume = int.MaxValue;
        }

        public void SetAudioMixerType(AudioMixerType audioMixerType)
        {
            _audioMixerType = audioMixerType;
        }
        
        public void SetFadeOut(float fadeOut)
        {
            _fadeOut = fadeOut;
        }

        public void Dispose()
        {
        }

        public void SetAudioConfigData(IAudioData audioData)
        {
            AudioData = audioData;
        }
    }
}
