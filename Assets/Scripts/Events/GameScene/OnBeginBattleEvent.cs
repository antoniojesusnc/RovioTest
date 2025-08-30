using RovioTest.Models;
using RovioTest.View;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBeginBattleEvent : IEventBusMessage
    {
        public CourtView Court { get; private set; }
        public CharacterView Player { get; private set; }
        public CharacterView Enemy { get; private set; }
        public BallView Ball { get; private set; }

        public OnBeginBattleEvent(CourtView court, CharacterView player, CharacterView enemy, BallView ball)
        {
            Court = court;
            Player = player;
            Enemy = enemy;
            Ball = ball;
        }
    }
}