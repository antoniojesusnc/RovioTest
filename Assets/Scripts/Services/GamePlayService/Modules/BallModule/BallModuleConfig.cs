using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RovioTest.Config
{
    public class BallModuleConfig : ScriptableObject
    {
        [field: Header("Ball Score Modification By Hit Type")]
        [field: SerializeField]
        public List<ModificationByHitType> ScoreModificationByHitType { get; private set; }
    }

    [Serializable]
    public class ModificationByHitType
    {
        [field: SerializeField]
        public BallHitTypes HitType { get; private set; }
        [field: FormerlySerializedAs("<Modification>k__BackingField")]
        [field: SerializeField]
        public float ScoreModificationRate { get; private set; }
        [field: FormerlySerializedAs("<Speed>k__BackingField")]
        [field: SerializeField]
        public float SpeedRateIncrease { get; private set; }
    }
}