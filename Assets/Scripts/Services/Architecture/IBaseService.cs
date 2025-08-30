using System;

namespace Urd.Services
{
    public interface IBaseService : IDisposable 
    {
        public int LoadPriority { get; }
        void Init();
        Type GetMainInterface();
    }
}