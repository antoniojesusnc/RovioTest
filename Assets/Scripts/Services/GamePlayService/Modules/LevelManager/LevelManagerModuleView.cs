using RovioTest.Events;
using RovioTest.Services;
using RovioTest.View;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.UI
{
    public class LevelManagerModuleView : MonoBehaviourEventObservable, IEventBusObservable<OnBeginBattleEvent>
    {
        [Header("Spawn")]
        [SerializeField] public Transform _courtParent;
        
        private CourtView _courtView;
        
        private LevelManagerModule _levelManager;

        protected override void Start()
        {
            base.Start();

            _levelManager = StaticServiceLocator.Get<IGamePlayService>().GetModule<LevelManagerModule>();
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            SetInitialPosition(newEvent.Court.transform, _courtParent);
            SetInitialPosition(newEvent.Player.transform, newEvent.Court.PlayerParent);
            SetInitialPosition(newEvent.Enemy.transform, newEvent.Court.EnemyParent);
        }

        private void SetInitialPosition(Transform element, Transform parent)
        {
            element.SetParent(parent, false);
            //element.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}