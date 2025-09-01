using System;
using System.Collections.Generic;
using MyBox;
using RovioTest.AI;
using UnityEngine;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new CharacterHitterConfig", menuName = "RovioTest/New CharacterHitterConfig", order = 1)]
    public class CharacterHitterConfig : ScriptableObject
    {
        [field: SerializeField]
        public int Difficulty { get; private set; }
        [field: SerializeReference, SubclassSelector]
        public ICharacterHitterBehavior HitterBehavior { get; private set; }
        
        [field: Header("Serve Chance")]
        [field: SerializeField]
        public List<CharacterHitterServeRate> HitServeRates { get; private set; }
    }
    
    [Serializable]
    public class CharacterHitterServeRate
    {
        [field: SerializeField, MinMaxRange(0, 1)]
        public MinMaxFloat HitRange { get; private set; }
        [field: SerializeField]
        public BallHitTypes HitType { get; private set; }
    }
}