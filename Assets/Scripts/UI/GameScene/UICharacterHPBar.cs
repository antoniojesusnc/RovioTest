using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using UnityEngine.UI;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterHPBar : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterBeingHitEvent>
    {
        [SerializeField] private Slider _slider;
        
        private CharacterView _characterView;

        protected override void Start()
        {
            base.Start();
            _characterView = GetComponentInParent<CharacterView>();
        }
        
        private void UpdateData()
        {
            _slider.value = _characterView.Model.HPRate;
        }

        public void OnNewEvent(OnCharacterBeingHitEvent newEvent)
        {
            if (newEvent.Character != _characterView)
            {
                return;
            }

            UpdateData();
        }
    }
}