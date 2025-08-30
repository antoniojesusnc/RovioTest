using DG.Tweening;
using RovioTest.Events;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UILookingForBattle : MonoBehaviourEventObservable, IEventBusObservable<OnBeginBattleEvent>
    {
        [SerializeField]
        private float _timeBeforeHide;
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
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}