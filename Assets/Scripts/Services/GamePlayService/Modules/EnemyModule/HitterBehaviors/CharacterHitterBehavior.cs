using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd.Services;

namespace RovioTest.AI
{
    public abstract class CharacterHitterBehavior : ICharacterHitterBehavior,
        IEventBusObservable<OnBallChangeObjectiveEvent>
    {
        private CharacterView _characterView; 
        private BallView _ballView;
        private IClockService _clockService;
        private IEventBusService _eventBusService;

        public bool _ballGoingToOpponent;
        
        public virtual void Dispose()
        {
            // TODO release managed resources here
        }

        public virtual void Begin(CharacterView characterView, BallView ballView)
        {
            _clockService = StaticServiceLocator.Get<IClockService>();
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            _eventBusService.Subscribe(this);
            
            _characterView = characterView;
            _ballView = ballView;
            _clockService.SubscribeToUpdate(CustomUpdate);
            _ballGoingToOpponent = false;
        }

        private void CustomUpdate(float deltaTime)
        {
            if (!IsBallCloseEnough() || _ballGoingToOpponent ) 
            {
                return;
            }
            
            if (!TryToHit())
            {
                return;
            }

            HitBall();
        }

        private void HitBall()
        {
            var ballDistance = Vector3.Distance(_ballView.transform.position, _characterView.transform.position);
            var hitType = _characterView.Model.GetHitType(ballDistance);
            if (!_ballView.IsMoving)
            {
                hitType = BallHitTypes.First;
            }

            var score = _characterView.Model.Config.Attack;
            
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_characterView, hitType));
        }

        protected abstract bool TryToHit();

        private bool IsBallCloseEnough()
        {
            return Vector3.Distance(_ballView.transform.position, _characterView.transform.position) < _characterView.Model.HitRadius;
        }

        public void Finish()
        {
            _eventBusService.Unsubscribe(this);
            _clockService?.UnSubscribeToUpdate(CustomUpdate);
            _clockService = null;
        }

        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            _ballGoingToOpponent = newEvent.SendToPlayer && newEvent.Objetive != _characterView;
        }
    }
}
