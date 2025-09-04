using Cinemachine;
using DG.Tweening;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Services;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest
{
    public class CameraController : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnCharacterHitBallEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private CinemachineMixingCamera _cameraMix;
        [SerializeField] private VFXVector3Shake _shake;

        private CharacterView _player;
        private CinemachineTransposer _transposer;

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _player = newEvent.LevelModel.PlayerView;
            if (_camera.Follow == null)
            {
                _camera.Follow = _player.transform;
                MakeInitialAnimation();
            }
        }

        private void MakeInitialAnimation()
        {
            var initialAnimationDuration = StaticServiceLocator.Get<IGamePlayService>().Config.InitialAnimationDuration;
            DOVirtual.Float(_cameraMix.m_Weight0, 0, initialAnimationDuration, OnUpdate).onComplete += OnComplete;
        }

        private void OnComplete()
        {
            _eventBusService.Send(new OnFinishInitialCameraAnimation());
        }

        private void OnUpdate(float value)
        {
            _cameraMix.m_Weight0 = value;
        }

        public void OnNewEvent(OnCharacterHitBallEvent newEvent)
        {
            ShakeScreen();
        }

        private void ShakeScreen()
        {
            _shake.DoEffect(_transposer.EffectiveOffset, ApplyShake);
        }

        private void ApplyShake(Vector3 newOffset)
        {
            _transposer.m_FollowOffset = newOffset;
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            ShakeScreen();
        }
    }
}