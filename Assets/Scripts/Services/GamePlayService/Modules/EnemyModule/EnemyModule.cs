using System;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest.Services
{
    [Serializable]
    public class EnemyModule : GamePlayModule, IEventBusObservable<OnBeginBattleEvent>
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
            var movementBehavior = _config.EnemyMovementTypes[0].MovementBehavior;
            _enemyModel.SetMovementBehavior(movementBehavior);
            
            _enemyModel.MovementBehavior.Begin(_enemyView);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _enemyView = newEvent.Enemy;
            _enemyModel = _enemyView.Model;
            _ballView = newEvent.Ball;

            AssignMovement();
        }
    }
}