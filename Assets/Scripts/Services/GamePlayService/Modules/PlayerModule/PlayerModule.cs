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
    public class PlayerModule : GamePlayModule, 
        IEventBusObservable<OnJoystickChangedEvent>, 
        IEventBusObservable<OnClickInCharacterSkillEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnBallChangeObjectiveEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>,
        IEventBusObservable<OnServeInputEvent>
    {
        private bool _detectJoystickInput = false;
        
        private CharacterView _playerView;
        private CharacterModel _playerModel;
        private BallView _ballView;

        private bool _ballGoingToOpponent;
        private bool _useSkillInNextHit;
        private bool _canMove;

        public override void GameOver(bool isWon)
        {
            _detectJoystickInput = false;
            
            base.GameOver(isWon);
        }

        public override void BeginBattle()
        {
            _detectJoystickInput = false;
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

            if (_useSkillInNextHit)
            {
                hitType = BallHitTypes.Skill;
                _useSkillInNextHit = false;
            }
            
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_playerView, hitType));
        }

        private void HitServe(BallHitTypes hitType)
        {
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_playerView, hitType, true));
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
            if (!_detectJoystickInput)
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
            _detectJoystickInput = false;
            _ballGoingToOpponent = false;
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            _canMove = true;
            _detectJoystickInput = true;
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            _playerView.Stop();
            _detectJoystickInput = false;
        }

        public void OnNewEvent(OnServeInputEvent newEvent)
        {
            HitServe(newEvent.HitType);
        }
    }
}