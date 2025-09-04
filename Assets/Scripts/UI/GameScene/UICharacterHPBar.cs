using RovioTest.Events;
using RovioTest.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterHPBar : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterBeingHitEvent>,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnFinishInitialCameraAnimation>
    {
        private const string HP_TEXT_FORMAT = "{0} / {1}";
        
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;
        
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
            _slider.value = _characterView.Model.HPRate;
            _text.SetText(HP_TEXT_FORMAT, _characterView.Model.CurrentHp, _characterView.Model.MaxHP); 
        }

        public void OnNewEvent(OnCharacterBeingHitEvent newEvent)
        {
            if (newEvent.Character != _characterView)
            {
                return;
            }

            UpdateData();
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            UpdateData();
        }

        public void OnNewEvent(OnFinishInitialCameraAnimation newEvent)
        {
            gameObject.SetActive(true);
        }
    }
}