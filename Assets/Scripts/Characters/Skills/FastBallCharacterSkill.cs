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
        [SerializeField] private float _ballSpeedIncreaseRate;
        [SerializeField] private float _ballSizeDeductionRate;

        public override void Begin(CharacterView owner)
        {
            base.Begin(owner);

            _skillsModule.Ball.transform.localScale *= _ballSizeDeductionRate;
            _skillsModule.Ball.Model.IncreaseSpeedRate(_ballSpeedIncreaseRate);
        }

        public override void Finish()
        {
            _skillsModule.Ball.transform.localScale /= _ballSizeDeductionRate;
            _skillsModule.Ball.Model.IncreaseSpeedRate(1 / _ballSpeedIncreaseRate);
            
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

            if (hitToFinish)
            {
                _skillsModule.FinishSkill(_owner);
            }
        }
    }
}