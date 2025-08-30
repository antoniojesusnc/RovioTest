using System;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    [Serializable]
    public class PlayerModule : GamePlayModule, IEventBusObservable<OnJoystickChangedEvent>, IEventBusObservable<OnBeginBattleEvent>
    {
        private bool _detectInput = false;
        
        private CharacterView _playerView;

        public override void GameOver(bool isWon)
        {
            _detectInput = false;
            
            base.GameOver(isWon);
        }

        public override void BeginBattle()
        {
            _detectInput = false;
            base.BeginBattle();
        }

        private void MoveCharacter(Vector2 newEventJoystickDelta)
        {
            _playerView?.Move(newEventJoystickDelta);
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
            TryHit();
        }

        private void TryHit()
        {
            Debug.Log("TryHit");
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>().PlayerView;
            
            _detectInput = true;
        }
    }
}