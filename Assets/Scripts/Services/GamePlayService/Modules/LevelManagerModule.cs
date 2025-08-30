using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest
{
    public class LevelManagerModule : GamePlayModule, IEventBusObservable<OnJoystickChangedEvent>
    {
        private CharacterView _characterView;

        public void SetCharacterView(CharacterView characterView)
        {
            _characterView = characterView;
        }
        
        private void MoveCharacter(Vector2 newEventJoystickDelta)
        {
            _characterView.Move(newEventJoystickDelta);
        }
        
        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            MoveCharacter(newEvent.joystickDelta);
        }

    }
}