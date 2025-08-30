using RovioTest.Models;
using UnityEngine;

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
    }
}