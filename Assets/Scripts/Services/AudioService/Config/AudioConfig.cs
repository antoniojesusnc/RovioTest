using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Urd/Services/Audio Config", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] 
        public float TimeToPlayAgain { get; private set; } = 0.05f;
        [field: SerializeField] public List<AudioMixerData> Mixers { get; private set; } = new List<AudioMixerData>();
        
        [field: SerializeReference, DisplayInspector]
        public List<AudioDataConfig> Audios { get; private set; } = new List<AudioDataConfig>();
        
        public AudioMixerGroup GetMixer(AudioMixerType mixerType)
        {
            return Mixers?.Find(mixer => mixer.MixerType == mixerType)?.Mixer;
        }
        public bool TryGetAudioData(AudioModel audioModel, out IAudioData audioData)
        {
            audioData = Audios.Find(audioDataConfig => audioDataConfig.AudioData.Type.Equals(audioModel.AudioType))?.AudioData;
            return audioData != null;
        }
    }
}