using Cinemachine;
using RovioTest.Events;
using UnityEngine;
using Urd;

namespace RovioTest
{
    public class CameraController : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginBattleEvent>
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _camera.Follow = newEvent.Player.transform;
        }
    }
}