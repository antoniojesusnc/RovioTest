using System;
using DG.Tweening;
using RovioTest.Events;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UIHitScreenColor : MonoBehaviourEventObservable, 
        IEventBusObservable<OnCharacterSmashedEvent>
    {
        [SerializeField]
        private float _effectDuration;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        private void DoEffect()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.DOFade(0, _effectDuration);
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            if (newEvent.CharacterDown.Model.IsPlayer)
            {
                DoEffect();
            }
        }
    }
}