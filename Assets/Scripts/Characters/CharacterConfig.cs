using UnityEngine;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new CharacterConfig", menuName = "RovioTest/New Character", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Stats")]
        [field: SerializeField]
        public float Hp { get; private set; }
        [field: SerializeField]
        public float SpeedMovement { get; private set; }
        [field: SerializeField]
        public float Attack { get; private set; }
        [field: SerializeField]
        public float BonusAttack { get; private set; }
        [field: SerializeField]
        public float Service { get; private set; }
        
        //[field: Header("Skills")]
    }
}