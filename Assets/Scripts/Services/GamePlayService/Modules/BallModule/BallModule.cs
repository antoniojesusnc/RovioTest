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

        public override void BeginGame()
        {
            base.BeginGame();
            _gameplayService = StaticServiceLocator.Get<IGamePlayService>();
        }

        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
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
            
            _ballView.Model.SetScore(newEvent.HitCharacter.Model.Attack);
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
            
            _ballView.Model.SetScore(score.RoundToInt());
            _ballView.Model.Hit();

            _hitter = newEvent.HitCharacter;
            var objetive = _hitter == _playerView ? _enemyView : _playerView;
            _eventBusService.Send(new OnBallChangeObjectiveEvent(objetive));
        }

        private void HitToCharacter(OnHitBallEvent newEvent)
        {
            
        }
    }
}