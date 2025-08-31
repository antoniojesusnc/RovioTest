using RovioTest.Events;
using RovioTest.Models;
using UnityEngine;
using Urd.Services;

namespace RovioTest.View
{
    public class CourtView : MonoBehaviour
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
        
        public void SetModel(CourtModel courtModel)
        {
            _courtModel = courtModel;
        }

        public void OnBallHit(Collision ballCollision)
        {
            StaticServiceLocator.Get<IEventBusService>()
                .Send(OnBallBeingHitEvent.HitWithWall(ballCollision.GetContact(0)));
        }
    }
}