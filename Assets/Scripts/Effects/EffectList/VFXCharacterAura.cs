using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new VFXCharacterAura", menuName = "RovioTest/Effects/New VFXCharacterAura",
        order = 1)]
    public class VFXCharacterAura : VFXEffectBase<Transform, Action<GameObject>>
    {
        [SerializeField]
        private AssetReferenceGameObject _particleSystem;

        protected override IEnumerator DoEffectCoroutine(Transform parent, Action<GameObject> callback)
        {
            var task = _particleSystem.InstantiateAsync(parent);
            yield return new WaitWhile(() => !task.IsDone);
            callback?.Invoke(task.Result);
        }

        public override void Cancel()
        {
            base.Cancel();
            
        }
    }
}