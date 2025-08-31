using RovioTest.Services;
using RovioTest.View;
using UnityEngine;
using Urd.Services;

namespace RovioTest.AI
{
    public abstract class CharacterMovementBehavior : ICharacterMovementBehavior
    {
        protected CharacterView _characterView;
        protected IClockService _clockService;
        protected bool _isMoving;
        protected bool _isStopped;

        public virtual void Begin(CharacterView characterView)
        {
            _characterView = characterView;

            _clockService = StaticServiceLocator.Get<IClockService>();
            _clockService.SubscribeToUpdate(CustomUpdate);
            
            _isStopped = false;
            _isMoving = false;
        }

        protected virtual void CustomUpdate(float deltaTime)
        {
            if (_isStopped)
            {
                return;
            }
            
            if(TryGetMovemenet(out var movementNormalized))
            {
                _characterView.Move(movementNormalized);
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }

        protected abstract bool TryGetMovemenet(out Vector2 movementNormalized);

        public virtual void Dispose()
        {
            _clockService.UnSubscribeToUpdate(CustomUpdate);
            _clockService = null;
            _characterView = null;
        }
        
        public virtual void Restart()
        {
            _isStopped = false;
            _isMoving = false;
        }
        
        public virtual void Continue()
        {
            _isStopped = false;
        }
        
        public virtual void Stop()
        {
            _isStopped = true;
        }

        public virtual void Finish()
        {
            _clockService.UnSubscribeToUpdate(CustomUpdate);
        }
    }
}