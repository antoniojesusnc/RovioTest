using DG.Tweening;
using RovioTest.Events;
using TMPro;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UIHitTypeMessage : MonoBehaviourEventObservable,
        IEventBusObservable<OnBallBeingHitEvent>
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Vector3 _playerOffset;
        
        private Camera _camera;

        protected override void Start()
        {
            base.Start();
            
            _camera = Camera.main;
            gameObject.SetActive(false);
        }

        public void ShowHitMessage(OnBallBeingHitEvent newEvent)
        {
            gameObject.SetActive(true);
            _text.SetText(newEvent.BallHitType.ToString());
            transform.position = newEvent.HitCharacter.transform.position + _playerOffset;
            transform.LookAt(_camera.transform.position);
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