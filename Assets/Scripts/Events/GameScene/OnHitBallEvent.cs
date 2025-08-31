using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnHitBallEvent : IEventBusMessage
    {
        public CharacterView HitCharacter { get; private set; }
        public BallHitTypes BallHitType { get; private set; }

        public static OnHitBallEvent HitByWall => new OnHitBallEvent(null, BallHitTypes.Wall);
        public OnHitBallEvent(CharacterView hitCharacter, BallHitTypes ballHitType)
        {
            HitCharacter = hitCharacter;
            BallHitType = ballHitType;
        }
    }
}