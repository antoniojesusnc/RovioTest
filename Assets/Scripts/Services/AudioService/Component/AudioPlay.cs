using System;
using UnityEngine;
using Urd.Services;

namespace Urd.Audio
{
    public class AudioPlay<T> : MonoBehaviour where T : Enum
    {
        [SerializeField]
        private T _audio;
        
        private IAudioService _audioService;

        void Awake()
        {
            _audioService = StaticServiceLocator.Get<IAudioService>();
        }
        
        public void Play()
        {
            _audioService.PlaySound(_audio);
        }
    }
}
