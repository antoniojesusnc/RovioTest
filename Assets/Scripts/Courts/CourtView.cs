using RovioTest.Models;
using UnityEngine;

namespace RovioTest.View
{
    public class CourtView : MonoBehaviour
    {
        [field: SerializeField] 
        public Transform PlayerParent { get; private set; }
        [field: SerializeField] 
        public Transform EnemyParent { get; private set; }

        private CourtModel _courtModel;
        
        public void SetModel(CourtModel courtModel)
        {
            _courtModel = courtModel;
        }
    }
}