using System.Collections.Generic;
using RovioTest.Events;
using RovioTest.Models;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.View
{
    public class CourtView : MonoBehaviourEventObservable,
        IEventBusObservable<OnBallBeingHitEvent>
    {
        [field: Header("Characters Positions")] 
        [field: SerializeField] 
        public Transform PlayerParent { get; private set; }
        [field: SerializeField] 
        public Transform EnemyParent { get; private set; }

        [field: Header("Ball Positions")]
        [field: SerializeField] 
        public Transform BallPlayer { get; private set; }
        [field: SerializeField] 
        public Transform BallEnemy { get; private set; }
        
        private CourtModel _courtModel;
        private List<WallTyre> _wallTyres;

        protected override void Start()
        {
            base.Start();
            _wallTyres = new List<WallTyre>(GetComponentsInChildren<WallTyre>());
        }

        public void SetModel(CourtModel courtModel)
        {
            _courtModel = courtModel;
        }

        public void OnBallHit(Collision ballCollision)
        {
            StaticServiceLocator.Get<IEventBusService>()
                .Send(OnBallBeingHitEvent.HitWithWall(ballCollision.GetContact(0)));
        }
        
        private void HitWithWall(ContactPoint contactPoint)
        {
            float closestPoint = float.MaxValue;
            int closestIndex = 0;
            var point = contactPoint.point;
            for (int i = 0; i < _wallTyres.Count; i++)
            {
                float newDistance = (_wallTyres[i].transform.position - point).sqrMagnitude;
                if (newDistance < closestPoint)
                {
                    closestPoint = newDistance;
                    closestIndex = i;
                }
            }
            
            _wallTyres[closestIndex].DoAnimation();
        }
        
        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            if (newEvent.BallHitType == BallHitTypes.Wall)
            {
                HitWithWall(newEvent.ContactPoint);
            }
        }
    }
}