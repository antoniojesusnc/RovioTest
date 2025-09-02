using System;
using RovioTest.Services;
using RovioTest.View;

namespace RovioTest.Skills
{
    public interface ICharacterSkill : IDisposable
    {
        bool IsActive { get; }
        void Init(SkillsModule skillsModule);
        void GetReady(CharacterView owner);
        void Begin(CharacterView owner);
        void Finish();
    }
}