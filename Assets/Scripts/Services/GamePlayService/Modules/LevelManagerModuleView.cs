using RovioTest.Events;
using RovioTest.Models;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest
{
    public class LevelManagerModuleView : MonoBehaviourEventObservable, IEventBusObservable<OnBeginBattleEvent>
    {
        [Header("Spawn")]
        [SerializeField] public Transform _playerParent;
        [SerializeField] public Transform _EnemyParent;
        [SerializeField] public Transform _courtParent;
        
        
        private LevelManagerModule _levelManager;
        private IEventBusService _eventService;

        private void Start()
        {
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
            SpawnCourt(newEvent.Court);
            SpawnPlayer(newEvent.Player);
            SpawnEnemy(newEvent.Player);
        }

        private void SpawnEnemy(CharacterModel newEventPlayer)
        {
            throw new System.NotImplementedException();
        }

        private void SpawnPlayer(CharacterModel newEventPlayer)
        {
            throw new System.NotImplementedException();
        }

        private void SpawnCourt(CourtModel newEventCourt)
        {
            throw new System.NotImplementedException();
        }
    }
}