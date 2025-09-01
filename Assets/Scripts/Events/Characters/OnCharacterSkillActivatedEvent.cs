using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterSkillActivatedEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterSkillActivatedEvent(CharacterView character)
        {
            Character = character;
        }
    }
}