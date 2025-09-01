using Cinemachine;
using DG.Tweening;
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
        IEventBusObservable<OnBeginServeEvent>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private CinemachineMixingCamera _cameraMix;

        private CharacterView _player;
        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _player = newEvent.LevelModel.PlayerView;
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            if (_camera.Follow == null)
            {
                _camera.Follow = _player.transform;
                MakeInitialAnimation();
            }
        }

        private void MakeInitialAnimation()
        {
            var initialAnimationDuration = StaticServiceLocator.Get<IGamePlayService>().Config.InitialAnimationDuration;
            DOVirtual.Float(_cameraMix.m_Weight0, 0, initialAnimationDuration, OnUpdate);
        }

        private void OnUpdate(float value)
        {
            _cameraMix.m_Weight0 = value;
        }
    }
}