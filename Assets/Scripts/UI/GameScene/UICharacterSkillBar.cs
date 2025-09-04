using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using UnityEngine.UI;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterSkillBar : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterHitBallEvent>,
        IEventBusObservable<OnFinishInitialCameraAnimation>
    {
        [SerializeField] private Slider _slider;
        
        private CharacterView _characterView;

        protected override void Start()
        {
            base.Start();
            _characterView = GetComponentInParent<CharacterView>();
            UpdateData();
            gameObject.SetActive(false);
        }
        
        private void UpdateData()
        {
            _slider.value = _characterView.Model.SkillPointsRate;
        }

        public void OnNewEvent(OnCharacterHitBallEvent newEvent)
        {
            if (newEvent.Character != _characterView)
            {
                return;
            }

            UpdateData();
        }

        public void OnNewEvent(OnFinishInitialCameraAnimation newEvent)
        {
            gameObject.SetActive(true);
        }
    }
}