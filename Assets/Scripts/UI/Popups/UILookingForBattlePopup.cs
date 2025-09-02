using DG.Tweening;
using RovioTest.Events;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UILookingForBattlePopup : MonoBehaviourEventObservable, 
        IEventBusObservable<OnBeginBattleEvent>
    {
        [SerializeField]
        private float _timeBeforeHide;
        [SerializeField]
        private float _fadeDuration;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            WaitAndFadeOut();
        }

        private void WaitAndFadeOut()
        {
            DOVirtual.DelayedCall(_timeBeforeHide, BeginFadeOut);
        }

        private void BeginFadeOut()
        {
            _canvasGroup.DOFade(0, _fadeDuration);
            gameObject.SetActive(false);
        }
    }
}