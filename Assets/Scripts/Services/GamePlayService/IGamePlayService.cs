using RovioTest.Models;
using Urd;
using Urd.Services;

namespace RovioTest
{
    public interface IGamePlayService : IBaseService
    {
        CharacterModel PlayerModel { get; }
        T GetModule<T>() where T : class, IGamePlayModule;
        void BeginBattle();
    }
}