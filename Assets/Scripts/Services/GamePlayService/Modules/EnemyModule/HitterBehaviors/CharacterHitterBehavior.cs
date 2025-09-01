using RovioTest.Config;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd.Services;

namespace RovioTest.AI
{
    public abstract class CharacterHitterBehavior : ICharacterHitterBehavior,
        IEventBusObservable<OnBallChangeObjectiveEvent>,
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnBeginServeEvent>
        
    {
        protected CharacterView _characterView; 
        protected BallView _ballView;
        protected CharacterHitterConfig _hitterBehaviorConfig;
        
        private IClockService _clockService;
        private IEventBusService _eventBusService;

        private bool _ballGoingToOpponent;
        private bool _useSkillInNextHit;
        private bool _canHit;

        public virtual void Dispose()
        {
            // TODO release managed resources here
        }

        public virtual void Begin(CharacterView characterView, CharacterHitterConfig hitterBehaviorConfig,
            BallView ballView)
        {
            _clockService = StaticServiceLocator.Get<IClockService>();
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            _eventBusService.Subscribe(this);

            _hitterBehaviorConfig = hitterBehaviorConfig;
            _characterView = characterView;
            _ballView = ballView;
            _clockService.SubscribeToUpdate(CustomUpdate);
            _ballGoingToOpponent = false;
            _useSkillInNextHit = false;
            _canHit = true;
        }

        public void Stop()
        {
            _canHit = false;
        }

        public void Restart()
        {
            _canHit = true;
        }

        public void Continue()
        {
            _canHit = true;
        }

        private void CustomUpdate(float deltaTime)
        {
            if (!_canHit)
            {
                return;
            }
            
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
            if (!_ballView.IsMoving)
            {
                HitFirstBall();
                return;
            }

            var hitType = GetHitType();
            
            if (_useSkillInNextHit)
            {
                hitType = BallHitTypes.Skill;
                _useSkillInNextHit = false;
            }
            
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_characterView, hitType));
        }

        protected abstract BallHitTypes GetHitType();
        protected abstract BallHitTypes GetServeHitType();

        private void HitFirstBall()
        {
            var hitType = GetServeHitType();
            _eventBusService.Send(OnBallBeingHitEvent.CharacterHitBall(_characterView, hitType, true));
        }

        protected abstract bool TryToHit();

        private bool IsBallCloseEnough()
        {
            return Vector3.Distance(_ballView.transform.position, _characterView.transform.position) < _characterView.Model.HitRadius;
        }

        public void Finish()
        {
            _canHit = false;
            _eventBusService.Unsubscribe(this);
            _clockService?.UnSubscribeToUpdate(CustomUpdate);
            _clockService = null;
        }
        
        private void CheckForSkill()
        {
            if (_characterView.Model.CanDoSkill)
            {
                ActivateSkill();
            }
        }
        
        private void ActivateSkill()
        {
            _eventBusService.Send(new OnCharacterSkillActivatedEvent(_characterView));
            _characterView.Model.ResetSkillPoints();
            _useSkillInNextHit = true;
        }

        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            _ballGoingToOpponent = newEvent.SendToPlayer && newEvent.Objetive != _characterView;
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            bool isValidHit = newEvent.BallHitType == BallHitTypes.Early
                              || newEvent.BallHitType == BallHitTypes.Good
                              || newEvent.BallHitType == BallHitTypes.Perfect
                              || newEvent.BallHitType == BallHitTypes.Late;
            if (newEvent.HitCharacter == _characterView && isValidHit)
            {
                CheckForSkill();
            }
        }


        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            _ballGoingToOpponent = false;
        }
    }
}
