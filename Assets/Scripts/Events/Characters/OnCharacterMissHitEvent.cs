using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterMissHitEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterMissHitEvent(CharacterView character)
        {
            Character = character;
        }
    }
}