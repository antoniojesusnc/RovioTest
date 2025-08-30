using System;
using Urd.Services;

namespace RovioTest
{
    [Serializable]
    public class GamePlayService : BaseService
    {
        public override int LoadPriority => ServicesPriority.Lowest;
    }
}