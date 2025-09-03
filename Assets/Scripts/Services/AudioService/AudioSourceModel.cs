using System;
using UnityEngine;

namespace Urd.Audio
{
    public class AudioSourceModel : IDisposable
    {
        public AudioSource AudioSource { get; private set; }
        public AudioModel AudioModel { get; private set; }

        public AudioSourceModel(AudioSource audioSource)
        {
            AudioSource = audioSource;
        }

        public void Dispose()
        {
            
        }
        
        public void SetAudioModel(AudioModel audioModel)
        {
            AudioModel = audioModel;
        }
        
        public void SetVolume(AudioMixerModel mixer)
        {
            AudioSource.volume = mixer.IsEnabled ? AudioModel.Volume : 0;
        }

    }
}
