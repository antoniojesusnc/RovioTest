using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterSkillBeginEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterSkillBeginEvent(CharacterView character)
        {
            Character = character;
        }
    }
}