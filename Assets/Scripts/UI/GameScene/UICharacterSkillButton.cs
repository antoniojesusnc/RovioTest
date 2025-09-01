using DG.Tweening;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using UnityEngine.UI;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterSkillButton : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterHitBallEvent>,
        IEventBusObservable<OnBeginBattleEvent>,
        IEventBusObservable<OnCharacterSkillActivatedEvent>
    {
        [SerializeField] private Image _grayImage;
        [SerializeField] private Button _button;
        [SerializeField] private DOTweenAnimation _animationWhenAvailable;
        
        private CharacterView _characterView;
        private bool _available = false;

        protected override void Start()
        {
            base.Start();
        }

        private void UpdateData()
        {
            _grayImage.fillAmount = 1 - _characterView.Model.SkillPointsRate;

            if (!_available && _characterView.Model.CanDoSkill)
            {
                _available = true;
                _animationWhenAvailable.tween.Restart();
            }

            _button.interactable = _available;
        }

        public void OnClickInSkill()
        {
            _available = false;
            UpdateData();
            
            _eventBusService.Send(new OnClickInCharacterSkillEvent(_characterView));
        }

        public void OnNewEvent(OnCharacterHitBallEvent newEvent)
        {
            if (newEvent.Character != _characterView)
            {
                return;
            }

            UpdateData();
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _characterView = newEvent.LevelModel.PlayerView;
            UpdateData();
        }

        public void OnNewEvent(OnCharacterSkillActivatedEvent newEvent)
        {
            if (_characterView == newEvent.Character)
            {
                UpdateData();
            }
        }
    }
}