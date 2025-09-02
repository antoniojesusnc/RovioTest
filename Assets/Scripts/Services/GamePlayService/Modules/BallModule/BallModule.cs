using System;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    [Serializable]
    public class BallModule : GamePlayModule, 
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnBallBeingHitEvent>
    {
        [field: SerializeField] 
        public BallModuleConfig Config { get; private set; }
        
        private CharacterView _hitter;
        
        private CharacterView _playerView;
        private CharacterView _enemyView;

        private BallView _ballView;
        private IGamePlayService _gameplayService;

        public override void BeginGame()
        {
            base.BeginGame();
            _gameplayService = StaticServiceLocator.Get<IGamePlayService>();
        }

        public override void BeginBattle()
        {
            base.BeginBattle();
        }

        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = newEvent.LevelModel.PlayerView;
            _enemyView = newEvent.LevelModel.EnemyView;
            _ballView = newEvent.LevelModel.BallView;

            _hitter = _playerView;
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            if (newEvent.IsServe)
            {
                HitServe(newEvent);
            }
            else
            {
                HandleHit(newEvent);
            }
        }

        private void HandleHit(OnBallBeingHitEvent newEvent)
        {
            switch (newEvent.BallHitType)
            {
                case BallHitTypes.Hit:
                    HitToCharacter(newEvent);
                    break;
                case BallHitTypes.Wall:
                    HitToWall(newEvent);
                    break;
                case BallHitTypes.Skill:
                    HitWithSkill(newEvent);
                    break;
                default:
                    CharacterHitBall(newEvent);
                    break;
            }
        }

        private void HitWithSkill(OnBallBeingHitEvent newEvent)
        {
            float score = newEvent.HitCharacter.Model.Attack;

            var hitConfig = Config.ScoreModificationByHitType
                .Find(hitType => hitType.HitType == newEvent.BallHitType);
            score *= hitConfig?.ScoreModificationRate ?? 0;
            
            newEvent.HitCharacter.Model.HitBall(score);
            
            _ballView.Model.IncreaseSpeedRate(hitConfig?.SpeedRateIncrease ?? 1);
            _ballView.Model.AddScore(score.RoundToInt());
            
            _eventBusService.Send(new OnCharacterHitBallEvent(newEvent.HitCharacter));
            
            SetBallToOpponent(newEvent.HitCharacter);
        }

        private void HitServe(OnBallBeingHitEvent newEvent)
        {
            _hitter = newEvent.HitCharacter;
            
            _ballView.Model.BeginMovement();
            
            float score = newEvent.HitCharacter.Model.Attack;
            var hitConfig = Config.ScoreModificationByHitType
                .Find(hitType => hitType.HitType == newEvent.BallHitType);
            score *= hitConfig?.ScoreModificationRate ?? 0;
            newEvent.HitCharacter.Model.HitBall(score);
            
            _ballView.Model.Hit();
            _ballView.Model.IncreaseSpeedRate(hitConfig?.SpeedRateIncrease ?? 1);
            _ballView.Model.AddScore(score.RoundToInt());

            var objective = _hitter == _playerView ? _enemyView : _playerView;
            var direction = (_ballView.transform.position-_hitter.transform.position).normalized;
            _eventBusService.Send(new OnFinishServeEvent());
            _eventBusService.Send(new OnCharacterHitBallEvent(newEvent.HitCharacter));
            _eventBusService.Send(new OnBallChangeObjectiveEvent(objective, direction));
        }

        private void HitToWall(OnBallBeingHitEvent newEvent)
        {
            var normal = newEvent.ContactPoint.normal;
            var direction = Vector3.Reflect(_ballView.transform.forward, normal);
            var objective = _hitter == _playerView ? _enemyView : _playerView;
            _eventBusService.Send(new OnBallChangeObjectiveEvent(objective, direction));
        }

        private void CharacterHitBall(OnBallBeingHitEvent newEvent)
        {
            float score = newEvent.HitCharacter.Model.Attack;
            var hitConfig = Config.ScoreModificationByHitType
                .Find(hitType => hitType.HitType == newEvent.BallHitType);
            score *= hitConfig?.ScoreModificationRate ?? 0;
            
            newEvent.HitCharacter.Model.HitBall(score);
            
            _eventBusService.Send(new OnCharacterHitBallEvent(newEvent.HitCharacter));
            
            _ballView.Model.IncreaseSpeedRate(hitConfig?.SpeedRateIncrease ?? 1);
            _ballView.Model.AddScore(score.RoundToInt());
            SetBallToOpponent(newEvent.HitCharacter);
        }

        private void HitToCharacter(OnBallBeingHitEvent newEvent)
        {
            newEvent.HitCharacter.Model.BeingHit(_ballView.Model.CurrentScore);
            _eventBusService.Send(new OnCharacterBeingHitEvent(newEvent.HitCharacter));
            _eventBusService.Send(new OnCharacterSmashedEvent(newEvent.HitCharacter)); 
            
            //_ballView.Model.ResetToInitialSpeed();
            //_ballView.Model.SetScore(newEvent.HitCharacter.Model.Attack);
            //SetBallToOpponent(newEvent.HitCharacter);
        }
        
        private void SetBallToOpponent(CharacterView hitCharacter)
        {
            _ballView.Model.Hit();

            _hitter = hitCharacter;
            var objetive = _hitter == _playerView ? _enemyView : _playerView;
            var direction = (_ballView.transform.position-_hitter.transform.position).normalized;
            _eventBusService.Send(new OnBallChangeObjectiveEvent(objetive, direction));
        }
    }
}