using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new Court", menuName = "RovioTest/New Court", order = 1)]
    public class CourtConfig : ScriptableObject
    {
        [field: Header("Asset")]
        [field: SerializeField]
        public AssetReferenceGameObject Asset { get; private set; } 
    }
}