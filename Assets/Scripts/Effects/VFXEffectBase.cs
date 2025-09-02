using System.Collections;
using UnityEngine;
using Urd.Services;

namespace RovioTest.Config
{
    
    public abstract class VFXEffectBase<T1, T2> : ScriptableObject, IVFXEffectBaseT2<T1, T2> 
    {
        protected Coroutine _coroutine;
        public void DoEffect(T1 classType1, T2 classType2)
        {
            _coroutine = StaticServiceLocator.Get<ICoroutineService>().StartCoroutine(DoEffectCoroutine(classType1, classType2));
        }

        public virtual void Cancel()
        {
            StaticServiceLocator.Get<ICoroutineService>().StopCoroutine(_coroutine);
        }
        protected abstract IEnumerator DoEffectCoroutine(T1 classType1, T2 classType2);
    }
    
    public abstract class VFXEffectBase<T> : ScriptableObject, IVFXEffectBaseT1<T> 
    {
        protected Coroutine _coroutine;
        public void DoEffect(T classType)
        {
            _coroutine = StaticServiceLocator.Get<ICoroutineService>().StartCoroutine(DoEffectCoroutine(classType));
        }

        public virtual void Cancel()
        {
            StaticServiceLocator.Get<ICoroutineService>().StopCoroutine(_coroutine);
        }
        protected abstract IEnumerator DoEffectCoroutine(T classType);
    }
    
    public abstract class VFXEffectBase : ScriptableObject, IVFXEffectBaseT0 
    {
        protected Coroutine _coroutine;
        public void DoEffect()
        {
            _coroutine = StaticServiceLocator.Get<ICoroutineService>().StartCoroutine(DoEffectCoroutine());
        }

        public virtual void Cancel()
        {
            StaticServiceLocator.Get<ICoroutineService>().StopCoroutine(_coroutine);
        }
        protected abstract IEnumerator DoEffectCoroutine();
    }
    
}