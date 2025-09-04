using DG.Tweening;
using RovioTest.Events;
using RovioTest.Services;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.UI
{
    public class UISmashedMessage : MonoBehaviourEventObservable,
        IEventBusObservable<OnCharacterSmashedEvent>
    {
        [SerializeField] private Vector3 _playerOffset;
        
        private Camera _camera;
        private LevelManagerModule _levelManagerModule;

        protected override void Start()
        {
            base.Start();
            
            _camera = Camera.main;
            gameObject.SetActive(false);
            transform.LookAt(_camera.transform.position);
            _levelManagerModule  = StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>();
        }

        public void ShowHitMessage(CharacterView characterSmashed)
        {
            bool isPlayer = characterSmashed.Model.IsPlayer;
            gameObject.SetActive(true);

            var position = isPlayer
                ? _levelManagerModule.LevelModel.CourtView.PlayerParent.transform.position
                : _levelManagerModule.LevelModel.CourtView.EnemyParent.transform.position;
            
            var textOffset = isPlayer
                ?_playerOffset
                : -1*_playerOffset;
            
            transform.position = position + textOffset;
            transform.LookAt(_camera.transform.position);
            DOTween.Restart(gameObject);
        }

        public void OnFinishAnimation()
        {
            gameObject.SetActive(false);
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
           ShowHitMessage(newEvent.CharacterDown);
        }
    }
}