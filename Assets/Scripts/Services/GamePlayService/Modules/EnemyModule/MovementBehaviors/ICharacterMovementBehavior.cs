using System;
using RovioTest.View;

namespace RovioTest.AI
{
    public interface ICharacterMovementBehavior : IDisposable
    {
        void Begin(CharacterView characterView);
        void Restart();
        void Stop();
        void Finish();
    }
}