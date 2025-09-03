using DG.Tweening;
using RovioTest.Events;
using TMPro;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.UI
{
    public class UIHitTypeMessage : MonoBehaviourEventObservable,
        IEventBusObservable<OnBallBeingHitEvent>
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Vector3 _playerOffset;
        [SerializeField] private DOTweenAnimation _movementAnimation;
        
        private Camera _camera;

        protected override void Start()
        {
            base.Start();
            
            _camera = Camera.main;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            StaticServiceLocator.Get<IClockService>()?.SubscribeToUpdate(CustomUpdate);
        }
        
        private void OnDisable()
        {
            StaticServiceLocator.Get<IClockService>()?.UnSubscribeToUpdate(CustomUpdate);
        }

        private void CustomUpdate(float deltaTime)
        {
            transform.LookAt(_camera.transform.position);
        }
        
        public void ShowHitMessage(OnBallBeingHitEvent newEvent)
        {
            gameObject.SetActive(true);
            _text.SetText(newEvent.BallHitType.ToString());
            transform.position = newEvent.HitCharacter.transform.position + _playerOffset;
            DOTween.Restart(gameObject);
        }

        public void OnFinishAnimation()
        {
            gameObject.SetActive(false);
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            bool isValidHit = newEvent.BallHitType == BallHitTypes.Early
                              || newEvent.BallHitType == BallHitTypes.Good
                              || newEvent.BallHitType == BallHitTypes.Perfect
                              || newEvent.BallHitType == BallHitTypes.Late;
            if (!newEvent.HitCharacter?.Model?.IsPlayer == true || !isValidHit)
            {
                return;
            }
            
            ShowHitMessage(newEvent);
        }
    }
}