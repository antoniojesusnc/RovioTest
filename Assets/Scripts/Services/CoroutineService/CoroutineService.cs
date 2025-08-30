using System;
using System.Collections;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class CoroutineService : BaseService, ICoroutineService
    {
        public override int LoadPriority => 10;
        
        private MonoBehaviour _coroutineBase;

        public override void Init()
        {
            base.Init();

            _coroutineBase = GameObject.FindAnyObjectByType<ServiceLocatorStarted>();
        }

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _coroutineBase.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _coroutineBase.StopCoroutine(coroutine);
        }

        public void StopAllCoroutines()
        {
            _coroutineBase.StopAllCoroutines();
        }
    }
}