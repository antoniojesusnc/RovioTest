using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest
{
    public class LevelManagerModuleView : MonoBehaviourEventObservable, IEventBusObservable<OnBeginBattleEvent>
    {
        [Header("Spawn")]
        [SerializeField] public Transform _courtParent;
        
        private CourtView _courtView;
        
        private LevelManagerModule _levelManager;
        private IEventBusService _eventService;

        protected override void Start()
        {
            base.Start();
            
            _levelManager =  StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>();
            _eventService =  StaticServiceLocator.Get<IEventBusService>();
        }

        private void MoveCharacter(Vector2 joystickDelta)
        {
            _eventService.Send(new OnJoystickChangedEvent(joystickDelta));
        }
        
        public void OnJoystickChanged(Vector2 joystickDelta)
        {
            MoveCharacter(joystickDelta);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            SetInitialPosition(newEvent.Court.transform, _courtParent);
            SetInitialPosition(newEvent.Player.transform, newEvent.Court.PlayerParent);
            SetInitialPosition(newEvent.Enemy.transform, newEvent.Court.EnemyParent);
        }

        private void SetInitialPosition(Transform element, Transform parent)
        {
            element.SetParent(parent, false);
            //element.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}