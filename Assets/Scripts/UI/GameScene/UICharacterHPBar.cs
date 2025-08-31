using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using UnityEngine.UI;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterHPBar : MonoBehaviourEventObservable,
        IEventBusObservable<OnHitBallEvent>
    {
        [SerializeField] private Slider _slider;
        
        private CharacterView _characterView;

        void Start()
        {
            _characterView = GetComponentInParent<CharacterView>();
        }
        
        private void UpdateData()
        {
            _slider.value = _characterView.Model.HPRate;
        }

        public void OnNewEvent(OnHitBallEvent newEvent)
        {
            if (newEvent.BallHitType != BallHitTypes.Hit 
                || newEvent.HitCharacter != _characterView)
            {
                return;
            }

            UpdateData();
        }
    }
}