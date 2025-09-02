using System.Collections;
using UnityEngine;
using Urd.Services;

namespace RovioTest.Config
{
    public abstract class VFXEffectBase<T> : ScriptableObject, IVFXEffectBase<T> 
    {
        protected Coroutine _coroutine;
        public void DoEffect(T classType)
        {
            _coroutine = StaticServiceLocator.Get<ICoroutineService>().StartCoroutine(DoEffectCoroutine(classType));
        }

        public void Cancel()
        {
            StaticServiceLocator.Get<ICoroutineService>().StopCoroutine(_coroutine);
        }
        protected abstract IEnumerator DoEffectCoroutine(T classType);
    }
}