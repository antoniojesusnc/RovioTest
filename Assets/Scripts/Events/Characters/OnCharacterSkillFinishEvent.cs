using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterSkillFinishEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnCharacterSkillFinishEvent(CharacterView character)
        {
            Character = character;
        }
    }
}