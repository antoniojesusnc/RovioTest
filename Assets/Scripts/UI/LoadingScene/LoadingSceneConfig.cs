using UnityEngine;

namespace RovioTest
{
    public class LoadingSceneConfig : ScriptableObject
    {
        [field: SerializeField]
        public float LoadingTime { get; private set; }
    }
}
