using System;
using DG.Tweening;
using MyBox;
using RovioTest.AI;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    [Serializable]
    public class EnemyModule : GamePlayModule, 
        IEventBusObservable<OnBeginBattleEvent>, 
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>,
        IEventBusObservable<OnCharacterHitAgainstWallEvent>
    {
        [SerializeField] 
        private EnemyModuleConfig _config;
        
        private CharacterView _enemyView;
        private CharacterModel _enemyModel;
        private BallView _ballView;
        private Tween _restartMovementDelay;

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
            var newHitterBehavior = _enemyModel.Config.DefaultHitterConfig;
            if (newHitterBehavior == null)
            {
                newHitterBehavior = _config.HitterBehaviors.GetRandom();
            }

            _enemyModel.SetHitterBehavior(newHitterBehavior.HitterBehavior);
            _enemyModel.HitterBehavior.Begin(_enemyView, newHitterBehavior, _ballView);
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
            if (newEvent.HitCharacter != _enemyView || newEvent.BallHitType == BallHitTypes.Hit)
            {
                return;
            }
            _enemyModel.MovementBehavior.Stop();
            _restartMovementDelay = DOVirtual.DelayedCall(_enemyModel.Config.HitAnimationDuration, _enemyModel.MovementBehavior.Restart);
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            _enemyModel.Restart();
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            _restartMovementDelay?.Kill();
            _enemyModel.Stop();
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            _restartMovementDelay?.Kill();
            _enemyModel.MovementBehavior.Stop();

            if (newEvent.Server == _enemyView)
            {
                float waitTimeToServe = StaticServiceLocator.Get<IGamePlayService>().Config.WaitTimeAfterEnemyServe;
                DOVirtual.DelayedCall(waitTimeToServe, _enemyModel.HitterBehavior.Restart);
            }
            else
            {
                _enemyModel.HitterBehavior.Restart();
            }
        }

        public void OnNewEvent(OnCharacterHitAgainstWallEvent newEvent)
        {
            if (newEvent.Character == _enemyView)
            {
                _enemyModel.MovementBehavior.Restart();
            }
        }
    }
}