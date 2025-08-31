using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnClickInCharacterSkillEvent : IEventBusMessage
    {
        public CharacterView Character { get; private set; }

        public OnClickInCharacterSkillEvent(CharacterView character)
        {
            Character = character;
        }
    }
}