using System.Collections;
using UnityEngine;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new VFXBeingHitEffect", menuName = "RovioTest/Effects/New VFXBeingHitEffect",
        order = 1)]
    public class VFXWhiteColorEffect : VFXEffectBase<MeshRenderer>
    {
        [SerializeField]
        private Material _hitMaterial;
        [SerializeField]
        private float _duration;

        protected override IEnumerator DoEffectCoroutine(MeshRenderer meshRenderer)
        {
            var materialOriginal = meshRenderer.material;
            var originalColor = materialOriginal.color;
            meshRenderer.material = _hitMaterial;
            _hitMaterial.color = Color.white;
            yield return new WaitForSeconds(_duration);
            meshRenderer.material = materialOriginal;
            meshRenderer.material.color = originalColor;
        }
    }
}