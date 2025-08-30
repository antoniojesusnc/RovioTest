using System;

namespace Urd
{
    public interface IGamePlayModule : IDisposable
    {
        void Init();
        void BeginGame();
        void BeginBattle();
        void GameOver(bool isWon);
    }
}
