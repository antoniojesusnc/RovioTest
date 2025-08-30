using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnHitBallEvent : IEventBusMessage
    {
        public int PlayerScore { get; private set; }
        public bool HitByCharacter { get; private set; }
        public BallHitTypes BallHitType { get; private set; }

        public static OnHitBallEvent HitByWall => new OnHitBallEvent(0, false, BallHitTypes.Wall);
        public OnHitBallEvent(int playerScore, bool hitByCharacter, BallHitTypes ballHitType)
        {
            PlayerScore = playerScore;
            HitByCharacter = hitByCharacter;
            BallHitType = ballHitType;
        }
    }
}