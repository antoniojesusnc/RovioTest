using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.Services;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.View
{
    public class CharacterAnimationsView : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnGameOverEvent>,
        IEventBusObservable<OnCharacterMissHitEvent>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _avatar;
        [Header("Effects")] [SerializeField] private Transform _effectParent;
        [SerializeField] private VFXCharacterWalkSmoke _walkEffect;
        [SerializeField] private float _walkEffectFrequency;

        private Vector3 _lastPosition;
        private CharacterView _characterView;


        private float _timestamp;
        private bool CanGenerateWalkEffect => _timestamp <= 0;
        private CharacterView Opponent => LevelModel.PlayerView == _characterView
            ? LevelModel.EnemyView
            : LevelModel.PlayerView;

        private LevelModel _levelModel;
        private LevelModel LevelModel
        {
            get
            {
                if (_levelModel == null)
                {
                    _levelModel = StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>()
                        .LevelModel;
                }
                return _levelModel;
            }
        } 
        
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
            if (_timestamp > 0)
            {
                _timestamp -= deltaTime;
            }

            if (_characterView.IsMoving)
            {
                RotateAvatarToMovementDirection();
            }

            _animator.SetBool(CharacterAnimationsUtils.Triggers.IsRunning, _characterView.IsMoving);
            _lastPosition = transform.position;
            CheckWalkParticle();
        }

        private void CheckWalkParticle()
        {
            if (_characterView.IsMoving && CanGenerateWalkEffect)
            {
                _timestamp = _walkEffectFrequency;
                _walkEffect.DoEffect(_effectParent.transform.position);
                StaticServiceLocator.Get<IAudioService>().PlaySound(RovioTestAudiosTypes.Walk);
            } 
        }

        private void PlayIdleAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Idle);
        private void PlayRunAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Run);
        private void PlayHitBallAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.BaseballStrikeShortAnimation);
        private void PlayFallDownAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.FallingDown);
        private void PlayBaseBallIdleAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.BaseballIdle);
        private void PlayVictoryAnimation() => PlayAnimationIfNotPlaying(CharacterAnimationsUtils.Animations.Victory);
        
        private void RotateAvatarToMovementDirection()
        {
            RotateToPosition(transform.position + (transform.position - _lastPosition).normalized);
        }
        
        private void RotateToPosition(Vector3 position)
        {
            var direction = (position - transform.position).normalized;
            _avatar.LookAt(transform.position + direction);
        }
        
        private void PlayAnimationIfNotPlaying(string animationName)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                _animator.Play(animationName);
            }
        }
        
        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _levelModel = newEvent.LevelModel;
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
                RotateAvatarToMovementDirection();
            }
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            bool validShot =
                newEvent.BallHitType == BallHitTypes.Early
                || newEvent.BallHitType == BallHitTypes.Good
                || newEvent.BallHitType == BallHitTypes.Perfect
                || newEvent.BallHitType == BallHitTypes.Late
                || newEvent.BallHitType == BallHitTypes.Skill;
            if (validShot && newEvent.HitCharacter == _characterView)
            {
                PlayHitBallAnimation();
                RotateToPosition(LevelModel.BallView.transform.position);
            }
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            if (_characterView == newEvent.CharacterDown)
            {
                RotateToPosition(LevelModel.BallView.transform.position);
                PlayFallDownAnimation();
            }
            else
            {
                PlayVictoryAnimation();
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

            RotateToPosition(Opponent.transform.position);
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            if (newEvent.IsWon && _characterView.Model.IsPlayer
                || !newEvent.IsWon && !_characterView.Model.IsPlayer)
            {
                PlayVictoryAnimation();
            }
        }

        public void OnNewEvent(OnCharacterMissHitEvent newEvent)
        {
            PlayHitBallAnimation();
            StaticServiceLocator.Get<IAudioService>().PlaySound(RovioTestAudiosTypes.MissHit);
        }
    }
}