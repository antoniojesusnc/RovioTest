using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnGameOverEvent : IEventBusMessage
    {
        public bool IsWon { get; private set; }

        public OnGameOverEvent(bool isWon)
        {
            IsWon = isWon;
        }
    }
}