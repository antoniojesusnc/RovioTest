using RovioTest.Models;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBeginBattleEvent : IEventBusMessage
    {
        public LevelModel LevelModel { get; private set; }

        public OnBeginBattleEvent(LevelModel levelModel)
        {
            LevelModel = levelModel;
        }
    }
}