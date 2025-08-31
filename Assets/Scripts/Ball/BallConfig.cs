using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new BallConfig", menuName = "RovioTest/New Ball", order = 1)]
    public class BallConfig : ScriptableObject
    {
        [field: Header("Stats")]
        [field: SerializeField]
        public float Speed { get; private set; }

        [field: SerializeField]
        public float MaxTurnDegreesAngle { get; private set; }

        [field: Header("Asset")]
        [field: SerializeField]
        public AssetReferenceGameObject Asset { get; private set; } 
    }
}