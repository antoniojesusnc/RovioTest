using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RovioTest.Config
{
    public class BallModuleConfig : ScriptableObject
    {
        [field: Header("Ball Speed Increase Per Hit")]
        [field: SerializeField]
        public float BallSpeedIncreaseRatePerHit { get; private set; }

        [field: Header("Ball Score Modification By Hit Type")]
        [field: SerializeField]
        public List<ScoreModificationByHitType> ScoreModificationByHitType { get; private set; }
    }

    [Serializable]
    public class ScoreModificationByHitType
    {
        [field: SerializeField]
        public BallHitTypes HitType { get; private set; }
        [field: SerializeField]
        public float Modification { get; private set; }
    }
}