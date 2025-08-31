using System;
using System.Collections.Generic;
using MyBox;
using RovioTest.Events;
using RovioTest.Skills;
using RovioTest.View;
using Urd;

namespace RovioTest.Services
{
    [Serializable]
    public class SkillsModule : GamePlayModule, 
        IEventBusObservable<OnBallBeingHitEvent>,
        IEventBusObservable<OnBeginBattleEvent>
    {
        public CourtView Court { get; private set; }
        public CharacterView Player { get; private set; }
        public CharacterView Enemy { get; private set; }
        public BallView Ball { get; private set; }
        
        private List<ICharacterSkill> _activeSkills = new ();
        
        public override void GameOver(bool isWon)
        {
            base.GameOver(isWon);
            
        }

        public override void BeginBattle()
        {
            base.BeginBattle();
        }
        
        private void BeginSkill(CharacterView characterView)
        {
            _activeSkills.Add(characterView.Model.CharacterSkill);
            characterView.Model.CharacterSkill.Begin(characterView);
            
            _eventBusService.Send(new OnCharacterSkillBeginEvent(characterView));
        }

        public void FinishSkill(CharacterView characterView)
        {
            if (_activeSkills.IsNullOrEmpty() || !characterView.Model.CharacterSkill.IsActive)
            {
                return;
            }
            
            characterView.Model.CharacterSkill.Finish();
            _eventBusService.Send(new OnCharacterSkillFinishEvent(characterView));
            _activeSkills.Remove(characterView.Model.CharacterSkill);
        }

        public void OnNewEvent(OnBallBeingHitEvent newEvent)
        {
            if (newEvent.BallHitType == BallHitTypes.Skill)
            {
                BeginSkill(newEvent.HitCharacter);
            }
        }

        public void OnNewEvent(OnBeginBattleEvent newEvent)
        {
            Court = newEvent.Court;
            Player = newEvent.Player;
            Enemy = newEvent.Enemy;
            Ball = newEvent.Ball;

            Player.Model.CharacterSkill.Init(this);
            Enemy.Model.CharacterSkill.Init(this);
        }
    }
}