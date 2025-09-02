using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new VFXCharacterWalkSmoke", menuName = "RovioTest/Effects/New VFXCharacterWalkSmoke",
        order = 1)]
    public class VFXCharacterWalkSmoke : VFXEffectBase<Vector3>
    {
        [SerializeField]
        private AssetReferenceGameObject _particleSystem;

        protected override IEnumerator DoEffectCoroutine(Vector3 position)
        {
            var task = _particleSystem.InstantiateAsync(position, Quaternion.identity);
            yield return new WaitWhile(() => !task.IsDone);
            var particleSystem = task.Result.GetComponentInChildren<ParticleSystem>();
            yield return new WaitWhile(() => particleSystem.IsAlive());
            GameObject.Destroy(particleSystem);
            particleSystem = null;
        }
    }
}