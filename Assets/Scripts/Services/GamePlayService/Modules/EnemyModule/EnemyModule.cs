using System;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest.Services
{
    [Serializable]
    public class EnemyModule : GamePlayModule, IEventBusObservable<OnBeginBattleEvent>, IEventBusObservable<OnHitBallEvent>
    {
        [SerializeField] 
        private EnemyModuleConfig _config;
        
        private CharacterView _enemyView;
        private CharacterModel _enemyModel;
        private BallView _ballView;

        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
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
            var hitterBehavior = _config.HitterBehaviors.GetRandom().HitterBehavior;
            _enemyModel.SetHitterBehavior(hitterBehavior);
            
            _enemyModel.HitterBehavior.Begin(_enemyView, _ballView);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _enemyView = newEvent.Enemy;
            _enemyModel = _enemyView.Model;
            _ballView = newEvent.Ball;

            AssignMovement();
            AssignHitter();
        }

        public void OnNewEvent(OnHitBallEvent newEvent)
        {
            if (newEvent.HitCharacter != _enemyView)
            {
                return;
            }
            
            _enemyModel.MovementBehavior.Restart();
        }
    }
}