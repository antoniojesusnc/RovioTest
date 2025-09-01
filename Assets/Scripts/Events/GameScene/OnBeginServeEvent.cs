using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBeginServeEvent : IEventBusMessage
    {
        public CharacterView Server { get; private set; }

        public OnBeginServeEvent(CharacterView server)
        {
            Server = server;
        }
    }
}