using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBallBeingHitEvent : IEventBusMessage
    {
        public CharacterView HitCharacter { get; private set; }
        public BallHitTypes BallHitType { get; private set; }

        public static OnBallBeingHitEvent BallBeingHitByWall => new OnBallBeingHitEvent(null, BallHitTypes.Wall);
        public OnBallBeingHitEvent(CharacterView hitCharacter, BallHitTypes ballHitType)
        {
            HitCharacter = hitCharacter;
            BallHitType = ballHitType;
        }
    }
}