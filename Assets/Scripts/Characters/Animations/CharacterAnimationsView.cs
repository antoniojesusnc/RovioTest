using RovioTest.Events;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.View
{
    public class CharacterAnimationsView : MonoBehaviourEventObservable,
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnGameOverEvent>
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _avatar;
        
        private Vector3 _lastPosition;
        private CharacterView _characterView;
        private CharacterView _opponent;
        
        protected override void Start()
        {
            base.Start();
            _characterView = GetComponentInParent<CharacterView>(); 
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
        }

        private void CustomUpdate(float deltaTime)
        {
            if (_characterView.IsMoving)
            {
                RotateAvatar();
            }
            
            _animator.SetBool(CharacterAnimationsUtils.Triggers.IsRunning, _characterView.IsMoving);
            _lastPosition = transform.position;
        }

        private void PlayIdleAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Idle);
        private void PlayRunAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Run);
        private void PlayHitBallAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.BaseballStrikeShortAnimation);
        private void PlayFallDownAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.FallingDown);
        private void PlayBaseBallIdleAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.BaseballIdle);
        private void PlayVictoryAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Victory);
        
        private void RotateAvatar()
        {
            var direction = (transform.position - _lastPosition).normalized;
            _avatar.LookAt(transform.position + direction);
        }
        
        private void RotateToOpponent()
        {
            var direction = (_opponent.transform.position - _characterView.transform.position).normalized; 
            _avatar.LookAt(transform.position + direction);
        }
        
        private void PlayAnimationIfNotPlaying(string animationName)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                _animator.Play(animationName);
            }
        }

        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            if (newEvent.IsPointerDown)
            {
                PlayIdleAnimation();
            }
            else
            {
                PlayRunAnimation();
                RotateAvatar();
            }
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            bool validShot = newEvent.BallHitType == BallHitTypes.Early
                                             || newEvent.BallHitType == BallHitTypes.Good
                                             || newEvent.BallHitType == BallHitTypes.Perfect
                                             || newEvent.BallHitType == BallHitTypes.Late
                                             || newEvent.BallHitType == BallHitTypes.Hit
                                             || newEvent.BallHitType == BallHitTypes.Skill;
            if (validShot)
            {
                PlayHitBallAnimation();
            }
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            if (_characterView != newEvent.CharacterDown)
            {
                PlayFallDownAnimation();
            }
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            if (newEvent.Server == _characterView)
            {
                PlayBaseBallIdleAnimation();
            }
            else
            {
                PlayIdleAnimation();
            }

            RotateToOpponent();
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            if (newEvent.IsWon && _characterView.Model.IsPlayer
                || !newEvent.IsWon && !_characterView.Model.IsPlayer)
            {
                PlayVictoryAnimation();
            }
        }
    }
}