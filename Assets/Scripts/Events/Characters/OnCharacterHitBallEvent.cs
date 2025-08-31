using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterHitBallEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterHitBallEvent(CharacterView character)
        {
            Character = character;
        }
    }
}