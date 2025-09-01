using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterSmashedEvent : IEventBusMessage
    {
        public CharacterView CharacterDown { get; private set; }

        public OnCharacterSmashedEvent(CharacterView characterDown)
        {
            CharacterDown = characterDown;
        }
    }
}