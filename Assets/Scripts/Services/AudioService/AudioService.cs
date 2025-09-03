using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Urd.Audio;

namespace Urd.Services
{
    [Serializable]
    public class AudioService : BaseService, IAudioService
    {
        [SerializeField]
        private AudioConfig _audioConfig;

        private AudioServiceView _audioServiceView;
        
        public override int LoadPriority => 100;

        private List<AudioMixerModel> _audioMixers = new List<AudioMixerModel>();
        private List<AudioSourceModel> _audioSourcesModels = new List<AudioSourceModel>();
        
        Dictionary<IAudioData, DateTime> _lastTimeThatSounds = new Dictionary<IAudioData, DateTime>();
        
        public override void Init()
        {
            base.Init();
            
            GetAudioServiceView();
            CreateMixersModels();
        }

        private void CreateMixersModels()
        {
            _audioMixers = new List<AudioMixerModel>();
            for (int i = 0; i < _audioConfig.Mixers.Count; i++)
            {
                var mixerModel = new AudioMixerModel(_audioConfig.Mixers[i]);
                mixerModel.OnEnabledChanged += (isEnabled) => OnMixerModelEnabledChanged(mixerModel);
                _audioMixers.Add(mixerModel);
            }
        }

        private void OnMixerModelEnabledChanged(AudioMixerModel mixerModel)
        {
            for (int i = 0; i < _audioSourcesModels.Count; i++)
            {
                var audioSource = _audioSourcesModels[i].AudioSource;
                if (audioSource.isPlaying && audioSource.outputAudioMixerGroup == mixerModel.MixerGroup)
                {
                    _audioSourcesModels[i].SetVolume(GetMixer(_audioSourcesModels[i].AudioModel.AudioMixerType));
                }
            }
        }

        public void SetConfig(AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;
        }

        private void GetAudioServiceView()
        {
            _audioServiceView = GameObject.FindObjectOfType<AudioServiceView>();
        }
        
        public AudioMixerModel GetMixer(AudioMixerType audioMixerType)
        {
            return _audioMixers.Find(model => model.Type == audioMixerType);
        }

        public void PlaySound(Enum audioType) => PlaySound(new AudioModel(audioType));

        public void PlaySound(AudioModel audioModel)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioData audioConfigData))
            {
                Debug.LogWarning($"Audio with enum {audioModel.AudioType} not Found");
                return;
            }

            audioModel.SetAudioConfigData(audioConfigData);
            
            if (_lastTimeThatSounds.TryGetValue(audioConfigData, out var lastTime) &&
                lastTime.AddSeconds(_audioConfig.TimeToPlayAgain) > DateTime.Now)
            {
                return;
            }

            PlayInternal(audioModel);
        }

        private void PlayInternal(AudioModel audioModel)
        {
            var audioSourceModel = GetAudioSourceModel(audioModel);
            if (audioSourceModel == null)
            {
                return;
            }
            audioSourceModel.SetAudioModel(audioModel);
            
            var audioSource = audioSourceModel.AudioSource;
            
            if (!audioModel.AdditionalClips.IsNullOrEmpty())
            {
                var value = UnityEngine.Random.Range(0, audioModel.AdditionalClips.Count + 1);
                if (value == 0)
                {
                    audioSource.clip = audioModel.Clip;
                }
                else
                {
                    audioSource.clip = audioModel.AdditionalClips[value-1];
                }
            }
            else
            {
                audioSource.clip = audioModel.Clip;
            }
            
            var mixer = GetAudioMixer(audioModel);

            audioSource.pitch = audioModel.Pitch;
            audioSource.loop = audioModel.Loop;
            audioSource.outputAudioMixerGroup = mixer.MixerGroup; 
            audioSourceModel.SetVolume(mixer);
            
            _lastTimeThatSounds[audioModel.AudioData] = DateTime.Now;
            audioSource.Play();
        }

        private AudioMixerModel GetAudioMixer(AudioModel audioModel)
        {
            return _audioMixers.Find(model => model.Type == audioModel.AudioMixerType);
        }

        private AudioSourceModel GetAudioSourceModel(AudioModel audioModel)
        {
            AudioSourceModel audioSourceModel = null;

            if (_audioServiceView == null)
            {
                return null;
            }
            
            Transform audioSourceLocation = _audioServiceView.transform;
            if (audioModel.AudioLocation != null)
            {
                audioSourceLocation = audioModel.AudioLocation;
            }

            var audioSources = audioSourceLocation.GetComponents<AudioSource>()?.ToList() ?? new List<AudioSource>();
            var audioSource = audioSources.Find(audioSource => !audioSource.isPlaying);
            if (audioSource == null)
            {
                audioSource = audioSourceLocation.gameObject.AddComponent<AudioSource>();
                audioSourceModel = new AudioSourceModel(audioSource);
                _audioSourcesModels.Add(audioSourceModel);
            }
            else
            {
                audioSourceModel = _audioSourcesModels.Find(model => model.AudioSource == audioSource);
            }
            
            return audioSourceModel;
        }


        public bool IsSoundOfType(Enum audioType) => IsSoundOfType(new AudioModel(audioType));

        public bool IsSoundOfType(AudioModel audioModel)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioData audioConfigData))
            {
                Debug.LogWarning($"Audio with enum {audioModel.AudioType} not Found");
                return false;
            }

            audioModel.SetAudioConfigData(audioConfigData);

            var audioSource = GetAudioSourceThatSounds(audioModel);
            return audioSource != null;
        }
        
        private AudioSource GetAudioSourceThatSounds(AudioModel audioModel)
        {
            Transform audioSourceLocation = _audioServiceView.transform;
            if (audioModel.AudioLocation != null)
            {
                audioSourceLocation = audioModel.AudioLocation;
            }

            var audioSources = audioSourceLocation.GetComponents<AudioSource>()?.ToList() ?? new List<AudioSource>();
            return audioSources.Find(audioSource => audioSource.isPlaying && audioSource.clip == audioModel.Clip);
        }

        public void StopSound(Enum audioType, Action onStopSound) => StopSound(new AudioModel(audioType), onStopSound);
        
        public void StopSound(AudioModel audioModel, Action onStopSound)
        {
            if (!_audioConfig.TryGetAudioData(audioModel, out IAudioData audioConfigData))
            {
                Debug.LogWarning($"Audio with enum {audioModel.AudioType} not Found");
                onStopSound?.Invoke();
                return;
            }
            audioModel.SetAudioConfigData(audioConfigData);

            var audioSource = GetAudioSourceThatSounds(audioModel);
            if (audioSource == null)
            {
                onStopSound?.Invoke();
                return;
            }

            if (audioModel.FadeOut <= 0)
            {
                audioSource.Stop();
                onStopSound?.Invoke();
                return;
            }

            audioSource.DOFade(0, audioModel.FadeOut).onComplete += () => OnFinishAudioFadeOut(audioModel, audioSource, onStopSound);
        }

        private void OnFinishAudioFadeOut(AudioModel audioModel, AudioSource audioSource, Action onStopSound)
        {
            audioSource.Stop();
            onStopSound?.Invoke();
        }
    }
}
