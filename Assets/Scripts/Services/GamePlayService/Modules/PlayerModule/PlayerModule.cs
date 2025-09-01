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
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnBallChangeObjectiveEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>
    {
        private bool _detectInput = false;
        
        private CharacterView _playerView;
        private CharacterModel _playerModel;
        private BallView _ballView;

        private bool _ballGoingToOpponent;
        private bool _useSkillInNextHit;
        private bool _canMove;

        public override void GameOver(bool isWon)
        {
            _detectInput = false;
            
            base.GameOver(isWon);
        }

        public override void BeginBattle()
        {
            _detectInput = false;
            _ballGoingToOpponent = false;
            _useSkillInNextHit = false;
            base.BeginBattle();
        }

        private void MoveCharacter(Vector2 newEventJoystickDeltaNormalized)
        {
            _playerView?.Move(newEventJoystickDeltaNormalized);
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
                _useSkillInNextHit = false;
            }
            
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_playerView, hitType));
        }
        
        private void ActivateSkill()
        {
            _playerModel.ResetSkillPoints();
            _useSkillInNextHit = true;
            
            _eventBusService.Send(new OnCharacterSkillActivatedEvent(_playerView));
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = newEvent.LevelModel.PlayerView;
            _playerModel = _playerView.Model;
            _ballView = newEvent.LevelModel.BallView;
        }
        
        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            if (!_detectInput)
            {
                return;
            }

            if (!newEvent.IsPointerDown)
            {
                StopCharacter();
            }
            else if(_canMove)
            {
                MoveCharacter(newEvent.JoystickDelta);
            }
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

            ActivateSkill();
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            _canMove = false;
            _detectInput = true;
            _ballGoingToOpponent = false;
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            _canMove = true;
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            _detectInput = false;
        }
    }
}