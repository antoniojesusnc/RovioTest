using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterHitAgainstWallEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterHitAgainstWallEvent(CharacterView character)
        {
            Character = character;
        }
    }
}