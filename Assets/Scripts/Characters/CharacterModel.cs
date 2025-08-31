using System;
using RovioTest.AI;
using RovioTest.Config;

namespace RovioTest.Models
{
    public class CharacterModel : IDisposable
    {
        public CharacterConfig Config { get; private set; }

        public int Attack => Config.Attack;
        public float HitRadius => Config.HitRadius;
        public float MaxHP => Config.Hp;
        public float HPRate => CurrentHp / MaxHP;
        public float MaxSkillPoints => Config.SkillPoints;
        public float SkillPointsRate => CurrentSkillPoints / MaxSkillPoints;
        public bool IsAlive => CurrentHp > 0;
        public float Speed => Config.Speed;

        public float CurrentSkillPoints { get; private set; }
        public float CurrentHp { get; private set; }
        
        public bool IsPlayer { get; private set; }
        
        public ICharacterMovementBehavior MovementBehavior { get; private set; }
        public ICharacterHitterBehavior HitterBehavior { get; private set; }
        

        public void Dispose()
        {
            
        }
        
        public void SetConfig(CharacterConfig config, bool isPlayer = false)
        {
            IsPlayer = isPlayer;
            Config = config;
         
            ResetStats();
        }
        
        public void ResetStats()
        {
            CurrentHp = MaxHP;
            CurrentSkillPoints = 0;
        }

        public void HitBall(float score)
        {
            CurrentSkillPoints += score;
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

        public void BeingHit(int damage)
        {
            CurrentHp -= damage; 
        }
    }
}