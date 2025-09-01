using System;
using MyBox;
using RovioTest.AI;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest.Services
{
    [Serializable]
    public class EnemyModule : GamePlayModule, 
        IEventBusObservable<OnBeginBattleEvent>, 
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>
    {
        [SerializeField] 
        private EnemyModuleConfig _config;
        
        private CharacterView _enemyView;
        private CharacterModel _enemyModel;
        private BallView _ballView;

        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
            
            _enemyModel.MovementBehavior.Finish();
            _enemyModel.HitterBehavior.Finish();
        }

        public override void BeginBattle()
        {
            base.BeginBattle();
        }

        private void AssignMovement()
        {
            var movementBehavior = _config.MovementsBehaviors.GetRandom().MovementBehavior;
            _enemyModel.SetMovementBehavior(movementBehavior);
            
            _enemyModel.MovementBehavior.Begin(_enemyView);
        }
        
        private void AssignHitter()
        {
            var hitterBehavior = _config.HitterBehaviors.Find(config => config.HitterType == EnemyHitterTypes.DEBUG)?.HitterBehavior;
            if (hitterBehavior == null)
            {
                Debug.Log("DEBUG Missing All Behavior");
                hitterBehavior = _config.HitterBehaviors.GetRandom().HitterBehavior;
            }
            _enemyModel.SetHitterBehavior(hitterBehavior);
            
            _enemyModel.HitterBehavior.Begin(_enemyView, _ballView);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _enemyView = newEvent.LevelModel.EnemyView;
            _enemyModel = _enemyView.Model;
            _ballView = newEvent.LevelModel.BallView;
            
            AssignHitter();
            AssignMovement();
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            if (newEvent.HitCharacter != _enemyView)
            {
                return;
            }
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            _enemyModel.Restart();
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            _enemyModel.Stop();
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            _enemyModel.HitterBehavior.Restart();
            _enemyModel.MovementBehavior.Stop();
        }
    }
}