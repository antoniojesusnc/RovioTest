using System;
using RovioTest.View;

namespace RovioTest.AI
{
    public interface ICharacterHitterBehavior : IDisposable
    {
        void Begin(CharacterView characterView, BallView ballView);
        void Finish();
    }
}
