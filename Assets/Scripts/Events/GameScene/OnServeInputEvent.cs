using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnServeInputEvent : IEventBusMessage
    {
        public BallHitTypes HitType { get; private set; }

        public OnServeInputEvent(BallHitTypes hitType)
        {
            HitType = hitType;
        }
    }
}