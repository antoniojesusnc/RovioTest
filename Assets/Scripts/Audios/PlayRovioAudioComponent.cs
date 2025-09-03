using System;
using RovioTest.Config;
using UnityEngine;
using Urd.Services;

namespace RovioTest.View
{
    [Serializable]
    public class PlayRovioAudioComponent : MonoBehaviour
    {
        [SerializeField] 
        private RovioTestAudiosTypes _audio;
        [SerializeField] 
        private bool _playOnAwake;

        private void Awake()
        {
            if (_playOnAwake)
            {
                PlaySound();
            }
        }

        public void PlaySound()
        {
            StaticServiceLocator.Get<IAudioService>().PlaySound(_audio);
        }
    }
}