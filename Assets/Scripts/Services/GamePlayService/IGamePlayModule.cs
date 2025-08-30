using System;

namespace Urd
{
    public interface IGamePlayModule : IDisposable
    {
        void Init();
        void BeginGame();
        void GameOver(bool isWon);
    }
}
