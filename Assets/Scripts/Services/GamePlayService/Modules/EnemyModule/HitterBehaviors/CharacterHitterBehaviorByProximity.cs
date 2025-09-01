using System;
using MyBox;
using RovioTest.Config;
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

        public override void Begin(CharacterView characterView, CharacterHitterConfig hitterBehaviorConfig, BallView ballView)
        {
            base.Begin(characterView, hitterBehaviorConfig, ballView);
            _timeStamp = 0;
        }

        protected override BallHitTypes GetHitType()
        {
            var ballDistance = Vector3.Distance(_ballView.transform.position, _characterView.transform.position);
            return _characterView.Model.GetHitType(ballDistance);
        }

        protected override BallHitTypes GetServeHitType()
        {
            return _hitterBehaviorConfig.HitServeRates.GetWeightedRandom(GetServeRate).HitType;
        }

        private double GetServeRate(HitTypeByRadius rate)
        {
            return rate.HitRange.Max - rate.HitRange.Min;
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
