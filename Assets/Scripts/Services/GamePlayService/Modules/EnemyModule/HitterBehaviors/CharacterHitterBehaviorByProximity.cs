using System;
using RovioTest.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RovioTest.AI
{
    [Serializable]
    public class CharacterHitterBehaviorByProximity : CharacterHitterBehavior
    {
        [SerializeField] 
        private float _hitRate;
        
        [SerializeField] 
        private float _coolDownToTryAgain;

        private float _timeStamp;

        public override void Begin(CharacterView characterView, BallView ballView)
        {
            base.Begin(characterView, ballView);
            _timeStamp = 0;
        }

        protected override bool TryToHit()
        {
            if (_coolDownToTryAgain > 0)
            {
                _timeStamp -= Time.deltaTime;
            }
            
            if (IsInCoolDown())
            {
                return false;
            }
            
            BeginCoolDown();
            
            return Random.value < _hitRate;
        }

        private bool IsInCoolDown()
        {
            return _timeStamp > 0;
        }
        
        private void BeginCoolDown()
        {
            _timeStamp = _coolDownToTryAgain;
        }
    }
}
