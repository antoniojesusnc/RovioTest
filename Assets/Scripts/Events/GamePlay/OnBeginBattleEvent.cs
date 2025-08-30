using RovioTest.Models;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBeginBattleEvent : IEventBusMessage
    {
        public CourtModel Court { get; private set; }
        public CharacterModel Player { get; private set; }
        public CharacterModel Enemy { get; private set; }

        public OnBeginBattleEvent(CourtModel court, CharacterModel player, CharacterModel enemy)
        {
            Court = court;
            Player = player;
            Enemy = enemy;
        }
    }
}