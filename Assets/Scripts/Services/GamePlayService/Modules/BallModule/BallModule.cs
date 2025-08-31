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
        IEventBusObservable<OnHitBallEvent>
    {
        [SerializeField] 
        private BallModuleConfig _config;
        
        private CharacterView _hitter;
        
        private CharacterView _playerView;
        private CharacterView _enemyView;

        private BallView _ballView;
        private IGamePlayService _gameplayService;

        bool _isGameOver = false;
        public override void BeginGame()
        {
            base.BeginGame();
            _gameplayService = StaticServiceLocator.Get<IGamePlayService>();
        }

        public override void BeginBattle()
        {
            base.BeginBattle();
            _isGameOver = false;
        }

        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
            _isGameOver = true;
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = newEvent.Player;
            _enemyView = newEvent.Enemy;
            _ballView = newEvent.Ball;

            _hitter = _playerView;
        }

        public void OnNewEvent(OnHitBallEvent newEvent)
        {
            switch (newEvent.BallHitType)
            {
                case BallHitTypes.First:
                    HitFirst(newEvent);
                    break;
                case BallHitTypes.Hit:
                    HitToCharacter(newEvent);
                    break;
                case BallHitTypes.Wall:
                    HitToWall(newEvent);
                    break;
                default:
                    CharacterHitBall(newEvent);
                    break;
            }
        }

        private void HitFirst(OnHitBallEvent newEvent)
        {
            _ballView.Model.BeginMovement();
            
            _ballView.Model.AddScore(newEvent.HitCharacter.Model.Attack);
            _ballView.Model.Hit();

            _hitter = _hitter == _playerView ? _enemyView : _playerView;
            _eventBusService.Send(new OnBallChangeObjectiveEvent(_hitter));
        }

        private void HitToWall(OnHitBallEvent newEvent)
        {
            
        }

        private void CharacterHitBall(OnHitBallEvent newEvent)
        {
            float score = newEvent.HitCharacter.Model.Attack;
            score *= _config.ScoreModificationByHitType
                .Find(hitType => hitType.HitType == newEvent.BallHitType)?.Modification ?? 0;

            _ballView.Model.IncreaseSpeedRate(_config.BallSpeedIncreaseRatePerHit);
            _ballView.Model.AddScore(score.RoundToInt());
            SetBallToOpponent(newEvent.HitCharacter);
        }

        private void HitToCharacter(OnHitBallEvent newEvent)
        {
            newEvent.HitCharacter.Model.Hit(_ballView.Model.CurrentScore);
            _eventBusService.Send(new OnCharacterBeingHitEvent(newEvent.HitCharacter));
            
            _ballView.Model.ResetToInitialSpeed();
            _ballView.Model.SetScore(newEvent.HitCharacter.Model.Attack);
            SetBallToOpponent(newEvent.HitCharacter);
        }
        
        private void SetBallToOpponent(CharacterView hitCharacter)
        {
            _ballView.Model.Hit();

            _hitter = hitCharacter;
            var objetive = _hitter == _playerView ? _enemyView : _playerView;
            _eventBusService.Send(new OnBallChangeObjectiveEvent(objetive));
        }
    }
}