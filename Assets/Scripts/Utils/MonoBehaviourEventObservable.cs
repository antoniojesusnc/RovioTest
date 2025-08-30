using UnityEngine;
using Urd.Services;
using Urd.Services.EventBus;

namespace Urd
{
    public class MonoBehaviourEventObservable : MonoBehaviour, IEventBusObservable<IEventBusMessage>
    {
        protected IEventBusService _eventBusService;
        
        protected virtual void Start()
        {
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            if (_eventBusService == null)
            {
                StaticServiceLocator.OnServiceInitialized += SubscribeEvents;
            }
            else
            {
                SubscribeEvents();
            }
        }

        public virtual void SubscribeEvents()
        {
            StaticServiceLocator.OnServiceInitialized -= SubscribeEvents;
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            _eventBusService.Unsubscribe(this);
            _eventBusService.Subscribe(this);
        }

        public virtual void UnsubscribeEvents()
        {
            _eventBusService?.Unsubscribe(this);
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeEvents();
        }

        public void OnNewEvent(IEventBusMessage newEvent)
        {
            
        }
    }
}