using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new CharacterConfig", menuName = "RovioTest/New Character", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Stats")]
        [field: SerializeField]
        public float Hp { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public int Attack { get; private set; }
        [field: SerializeField]
        public float BonusAttack { get; private set; }
        [field: SerializeField]
        public float Service { get; private set; }
        
        [field: SerializeField]
        public float HitRadius { get; private set; }
        
        [field: SerializeField]
        public List<HitTypeByRadius> HitTypeByRadius { get; private set; }

        [field: SerializeField]
        public float CoolDownAfterMiss { get; private set; }
        
        [field: Header("Asset")]
        [field: SerializeField]
        public AssetReferenceGameObject Asset { get; private set; }
        //[field: Header("Skills")]
    }

    [Serializable]
    public class HitTypeByRadius
    {
        [field: SerializeField, MinMaxRange(0, 1)]
        public MinMaxFloat HitRange { get; private set; }
        [field: SerializeField]
        public BallHitTypes HitType { get; private set; }
    }
}