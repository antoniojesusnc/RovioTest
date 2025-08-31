using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterBeingHitEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterBeingHitEvent(CharacterView character)
        {
            Character = character;
        }
    }
}