using System;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest.Services
{
    [Serializable]
    public class PlayerModule : GamePlayModule, 
        IEventBusObservable<OnJoystickChangedEvent>, 
        IEventBusObservable<OnClickInCharacterSkillEvent>,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnBallChangeObjectiveEvent>
    {
        private bool _detectInput = false;
        
        private CharacterView _playerView;
        private CharacterModel _playerModel;
        private BallView _ballView;

        private bool _ballGoingToOpponent;
        private bool _useSkillInNextHit;
        
        public override void GameOver(bool isWon)
        {
            _detectInput = false;
            
            base.GameOver(isWon);
        }

        public override void BeginBattle()
        {
            _detectInput = false;
            _ballGoingToOpponent = false;
            base.BeginBattle();
        }

        private void MoveCharacter(Vector2 newEventJoystickDeltaNormalized)
        {
            _playerView?.Move(newEventJoystickDeltaNormalized);
        }
        
        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            if (!_detectInput)
            {
                return;
            }

            if (_playerView.IsMoving && !newEvent.IsPointerDown)
            {
                StopCharacter();
            }
            else
            {
                MoveCharacter(newEvent.JoystickDelta);
            }
        }

        private void StopCharacter()
        {
            _playerView.Stop();
            TryHitBall();
        }

        private void TryHitBall()
        {
            if (_ballGoingToOpponent)
            {
                return;
            }
                
            var ballDistance = Vector3.Distance(_ballView.transform.position, _playerView.transform.position);
            if (ballDistance > _playerModel.HitRadius)
            {
                return;
            }

            var hitType = _playerModel.GetHitType(ballDistance);
            if (!_ballView.IsMoving)
            {
                hitType = BallHitTypes.First;
            }

            if (_useSkillInNextHit)
            {
                hitType = BallHitTypes.Skill;
            }
            
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_playerView, hitType));
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = newEvent.Player;
            _playerModel = _playerView.Model;
            _ballView = newEvent.Ball;
            
            _detectInput = true;
        }
        
        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            _ballGoingToOpponent = newEvent.SendToPlayer && newEvent.Objetive != _playerView;
        }

        public void OnNewEvent(OnClickInCharacterSkillEvent newEvent)
        {
            if (newEvent.Character != _playerView)
            {
                return;
            }
            _useSkillInNextHit = true;
        }
    }
}