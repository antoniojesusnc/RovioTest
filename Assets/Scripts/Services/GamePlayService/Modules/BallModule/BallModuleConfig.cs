using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using RovioTest.Config;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Object = UnityEngine.Object;

namespace RovioTest
{
    public class BallModuleConfig : ScriptableObject
    {


        [field: Header("Ball Speed Increase Per Hit")]
        [field: SerializeField]
        public float BallSpeedIncreasePerHit { get; private set; }

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