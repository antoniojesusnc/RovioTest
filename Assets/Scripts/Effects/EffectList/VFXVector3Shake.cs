using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace RovioTest.Config
{
    [CreateAssetMenu(fileName = "new VFXVector3Shake", menuName = "RovioTest/Effects/New VFXVector3Shake",
        order = 1)]
    public class VFXVector3Shake : VFXEffectBase<Vector3, Action<Vector3>>
    {
        [SerializeField]
        private float _duration;

        [SerializeField] private float _strength = 3f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _ignoreZAxis = true;
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
        
        protected override IEnumerator DoEffectCoroutine(Vector3 initialVector3, Action<Vector3> callbackPerShake)
        {
            DOTween.Shake(() => initialVector3, 
                newValue => callbackPerShake?.Invoke(newValue),
                _duration,
                _strength,
                _vibrato,
                _randomness,
                _ignoreZAxis,
                _fadeOut,
                _randomnessMode);
            
            yield return 0;
        }
    }
}