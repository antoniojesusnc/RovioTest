using DG.Tweening;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterServe : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnBeginBattleEvent>
    {
        [SerializeField]
        private DOTweenAnimation _animation;
        
        private CharacterView _playerView;

        protected override void Start()
        {
            base.Start();
            Hide();
        }

        private void BeginServe()
        {
            _animation.tween.Restart();
        }
        
        public void OnClick()
        {
            if(!_animation.tween.IsPlaying())
            {
                BeginServe();
                return;
            }
            _animation.tween.Pause();
            var hitType = GetHitType();
            _eventBusService.Send(new OnServeInputEvent(hitType));

            Hide();
        }

        private void Hide()
        {
            _animation.tween.Rewind();
            gameObject.SetActive(false);
        }

        private BallHitTypes GetHitType()
        {
            var position = _animation.tween.position;
            if (_animation.tween.IsLoopingOrExecutingBackwards())
            {
                position = 1 - position;
            }
            
            CharacterHitterServeRate hitRateConfig = _playerView.Model.Config.HitServeRates[0];
            for (int i = 0; i < _playerView.Model.Config.HitServeRates.Count; i++)
            {
                hitRateConfig = _playerView.Model.Config.HitServeRates[i];
                if (position > hitRateConfig.HitRange.Min && position < hitRateConfig.HitRange.Max)
                {
                    break;
                }
            }

            return hitRateConfig.HitType;
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            BeginServe();
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            _playerView = newEvent.LevelModel.PlayerView;
        }
    }
}