using System;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest
{
    [Serializable]
    public class LevelManagerModule : GamePlayModule, IEventBusObservable<OnJoystickChangedEvent>
    {
        [SerializeField]
        private LevelManagerConfig _config;
        
        private CharacterView _characterView;

        public CourtView _courtModel;
        public CharacterView _playerView;
        public CharacterView _enemyView;
        
        public void SetCharacterView(CharacterView characterView)
        {
            _characterView = characterView;
        }
        
        private void MoveCharacter(Vector2 newEventJoystickDelta)
        {
            _characterView.Move(newEventJoystickDelta);
        }

        public override void BeginGame()
        {
            base.BeginGame();

            LoadAssetForBattle();
        }

        private void LoadAssetForBattle()
        {
            var court = _config.Courts.GetRandom();
            LoadCourt(court);
        }

        private void LoadCourt(CourtConfig court)
        {
            
        }

        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            MoveCharacter(newEvent.joystickDelta);
        }
    }
}