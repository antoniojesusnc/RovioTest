using RovioTest.Config;
using RovioTest.Models;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    public interface IGamePlayService : IBaseService
    {
        CharacterModel PlayerModel { get; }
        GamePlayConfig Config { get; }
        T GetModule<T>() where T : class, IGamePlayModule;
        void BeginGame();
        void BeginBattle();
        void GameOver(bool isWon);
    }
}