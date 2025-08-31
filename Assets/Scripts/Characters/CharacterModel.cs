using System;
using RovioTest.AI;
using RovioTest.Config;
using UnityEngine;

namespace RovioTest.Models
{
    public class CharacterModel : IDisposable
    {
        public CharacterConfig Config { get; private set; }

        [field: SerializeField] public float CurrentHp { get; private set; }
        public int Attack => Config.Attack;
        public float HitRadius => Config.HitRadius;

        public float MaxHP => Config.Hp;
        
        public float HPRate => CurrentHp / MaxHP;
        public float Speed => Config.Speed;

        public ICharacterMovementBehavior MovementBehavior { get; private set; }
        public ICharacterHitterBehavior HitterBehavior { get; private set; }
        
        public void Dispose()
        {
            
        }
        
        public void SetConfig(CharacterConfig config)
        {
            Config = config;
        }

        public BallHitTypes GetHitType(float ballDistance)
        {
            var ballDistanceNormalized = ballDistance / HitRadius;
            return Config.HitTypeByRadius.Find(radiusData =>
                radiusData.HitRange.Min < ballDistanceNormalized && radiusData.HitRange.Max > ballDistanceNormalized)?.HitType  
                   ?? BallHitTypes.None;
        }

        public void SetMovementBehavior(ICharacterMovementBehavior movementBehavior)
        {
            MovementBehavior = movementBehavior;
        }

        public void SetHitterBehavior(ICharacterHitterBehavior hitterBehavior)
        {
            HitterBehavior = hitterBehavior;
        }
    }
}