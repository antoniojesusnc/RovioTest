using RovioTest.Events;
using RovioTest.Services;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.UI
{
    public class UIGameOverPopup : MonoBehaviourEventObservable, 
        IEventBusObservable<OnGameOverEvent>
    {
        [SerializeField]
        public GameObject _wonImage;
        [SerializeField]
        public GameObject _looseImage;
        
        private bool _isWon;

        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            _isWon= newEvent.IsWon;
            Show();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            
            _wonImage.SetActive(_isWon);
            _looseImage.SetActive(!_isWon);
        }

        public void OnClickInContinue()
        {
            StaticServiceLocator.Get<IGamePlayService>().BeginGame();
        }
    }
}