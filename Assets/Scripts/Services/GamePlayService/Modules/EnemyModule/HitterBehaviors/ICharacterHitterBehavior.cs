using System;
using RovioTest.Config;
using RovioTest.View;

namespace RovioTest.AI
{
    public interface ICharacterHitterBehavior : IDisposable
    {
        void Begin(CharacterView characterView, CharacterHitterConfig hitterBehaviorConfig, BallView ballView);
        void Stop();
        void Restart();
        void Continue();
        void Finish();
    }
}
