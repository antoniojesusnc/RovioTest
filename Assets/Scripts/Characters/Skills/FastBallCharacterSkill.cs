using System;
using RovioTest.Events;
using RovioTest.View;
using UnityEngine;

namespace RovioTest.Skills
{
    [Serializable]
    public class FastBallCharacterSkill : CharacterSkill,
        IEventBusObservable<OnBallBeingHitEvent>
    {
        [field: SerializeField] 
        public float BallSpeedIncreaseRate { get; private set; }
        [field: SerializeField] 
        public float BallSizeDeductionRate { get; private set; }
        
        public FastBallCharacterSkill(ICharacterSkill skill) : base(skill)
        {
            var skillData = skill as FastBallCharacterSkill;
            BallSpeedIncreaseRate = skillData.BallSpeedIncreaseRate;
            BallSizeDeductionRate = skillData.BallSizeDeductionRate;
        }

        public override void Begin(CharacterView owner)
        {
            base.Begin(owner);
            
            _skillsModule.LevelModel.BallView.ChangeScale(BallSizeDeductionRate);
            _skillsModule.LevelModel.BallView.Model.IncreaseSpeedRate(BallSpeedIncreaseRate);
        }

        public override void Finish()
        {
            _skillsModule.LevelModel.BallView.ChangeScale(1/BallSizeDeductionRate);
            _skillsModule.LevelModel.BallView.Model.IncreaseSpeedRate(1 / BallSpeedIncreaseRate);
            
            base.Finish();
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            bool hitToFinish = newEvent.BallHitType == BallHitTypes.Early
                               || newEvent.BallHitType == BallHitTypes.Good
                               || newEvent.BallHitType == BallHitTypes.Perfect
                               || newEvent.BallHitType == BallHitTypes.Late
                               || newEvent.BallHitType == BallHitTypes.Hit
                               || newEvent.BallHitType == BallHitTypes.Skill;

            if (hitToFinish && newEvent.HitCharacter != _owner)
            {
                _skillsModule.FinishSkill(_owner);
            }
        }
    }
}