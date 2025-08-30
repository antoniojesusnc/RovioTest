using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnCharacterDownEvent : IEventBusMessage
    {
        public CharacterView CharacterDown { get; private set; }

        public OnCharacterDownEvent(CharacterView characterDown)
        {
            CharacterDown = characterDown;
        }
    }
}