using System.Collections.Generic;
using DG.Tweening;
using RovioTest.Config;
using RovioTest.Events;
using UnityEngine;
using Urd;
using Urd.Audio;
using Urd.Feedback;
using Urd.Services;

namespace RovioTest.View
{
    public class PlayGeneralFeedbackView : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterHitBallEvent>,
        IEventBusObservable<OnCharacterSkillBeginEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnGameOverEvent>,
        IEventBusObservable<OnBeginBattleEvent>
    {
        [SerializeField] 
        private int _publicAtSameTime;
        [SerializeField] 
        private float _delaySecondsToBeginAnotherPublic;
        private int _publicSoundPlaying;
        
        private IAudioService _audioService;
        private List<Tween> _timersForPlayPublic = new List<Tween>();
        private IPhysicalFeedbackService _physicalService;

        protected override void Start()
        {
            base.Start();
            _audioService = StaticServiceLocator.Get<IAudioService>();
            _physicalService = StaticServiceLocator.Get<IPhysicalFeedbackService>();
        }

        public void OnNewEvent(OnCharacterHitBallEvent newEvent)
        {
            _audioService.PlaySound(RovioTestAudiosTypes.HitNormal);
            _physicalService.Haptic(HapticType.Light);
        }

        public void OnNewEvent(OnCharacterSkillBeginEvent newEvent)
        {
            _audioService.PlaySound(RovioTestAudiosTypes.HitSkill);
            _physicalService.Haptic(HapticType.Medium);
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            _audioService.PlaySound(RovioTestAudiosTypes.Smash);
            _physicalService.Haptic(HapticType.Medium);
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            if (newEvent.BallHitType == BallHitTypes.Wall)
            { 
                _audioService.PlaySound(RovioTestAudiosTypes.BounceInCourt);
                _physicalService.Haptic(HapticType.Light);
            }
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            if (newEvent.IsWon)
            {
                _audioService.PlaySound(RovioTestAudiosTypes.Victory);
            }
            else
            {
                _audioService.PlaySound(RovioTestAudiosTypes.Defeat);
            }
            
            _audioService.StopSound(RovioTestAudiosTypes.Public);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            TryToPlayPublic();
        }

        private void TryToPlayPublic()
        {
            if (_publicSoundPlaying >= _publicAtSameTime)
            {
                return;
            }

            ++_publicSoundPlaying;
            var publicAudioModel = new AudioModel(RovioTestAudiosTypes.Public);
            _audioService.PlaySound(publicAudioModel);
            _timersForPlayPublic.Add(DOVirtual.DelayedCall(publicAudioModel.Clip.length, OnFinishPublicAudio));
            
            if (_publicSoundPlaying < _publicAtSameTime)
            {
                _timersForPlayPublic.Add(DOVirtual.DelayedCall(_delaySecondsToBeginAnotherPublic, TryToPlayPublic));
            } 
        }

        private void OnFinishPublicAudio()
        {
            _timersForPlayPublic.RemoveAll(tween => tween == null || !tween.IsActive());
            --_publicSoundPlaying;
            TryToPlayPublic();
        }
    }
}