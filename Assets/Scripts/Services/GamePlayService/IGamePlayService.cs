using Urd;
using Urd.Services;

namespace RovioTest
{
    public interface IGamePlayService : IBaseService
    {
        public T GetModule<T>() where T : class, IGamePlayModule;
    }
}