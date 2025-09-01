using RovioTest.Events;
using RovioTest.Models;
using RovioTest.Services;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.UI
{
    public class LevelManagerModuleView : MonoBehaviourEventObservable, 
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginServeEvent>
        
    {
        LevelModel _levelModel;
        
        private enum InitialPosition
        {
            Player,
            Enemy
        }
        
        [Header("Spawn")]
        [SerializeField] public Transform _courtParent;
        
        private CourtView _courtView;
        
        private LevelManagerModule _levelManager;

        protected override void Start()
        {
            base.Start();

            _levelManager = StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>();
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _levelModel = newEvent.LevelModel;
            SetInitialPositionFor(InitialPosition.Player);
        }

        private void SetInitialPositionFor(InitialPosition whoServe)
        {
            SetInitialPosition(_levelModel.CourtView.transform, _courtParent);
            SetInitialPosition(_levelModel.PlayerView.transform, _levelModel.CourtView.PlayerParent);
            SetInitialPosition(_levelModel.EnemyView.transform, _levelModel.CourtView.EnemyParent);
            if (whoServe == InitialPosition.Player)
            {
                SetInitialPosition(_levelModel.BallView.transform, _levelModel.CourtView.BallPlayer);
            }
            else
            {
                SetInitialPosition(_levelModel.BallView.transform, _levelModel.CourtView.BallEnemy);
            }
        }

        private void SetInitialPosition(Transform element, Transform parent)
        {
            element.SetParent(parent, false);
            element.localPosition = Vector3.zero;
            element.localRotation = Quaternion.identity;
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            SetInitialPositionFor(newEvent.Server == _levelModel.PlayerView?InitialPosition.Player: InitialPosition.Enemy );
        }
    }
}