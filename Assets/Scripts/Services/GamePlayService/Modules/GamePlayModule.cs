using System.Collections;
using System.Collections.Generic;
using Urd.Services;
using Urd.Services.EventBus;

namespace Urd
{
    public abstract class GamePlayModule : IGamePlayModule, IEventBusObservableBase
    {
        protected IEventBusService _eventBusService;

        public virtual void Init()
        {
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
        }

        public virtual void Dispose()
        {
            _eventBusService?.Unsubscribe(this);
        }

        public virtual void BeginGame()
        {
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            _eventBusService.Subscribe(this);
        }

        public virtual void BeginBattle()
        {
            
        }

        public virtual void GameOver(bool isWon)
        {
            
        }
    }
}
