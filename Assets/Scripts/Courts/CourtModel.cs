using System;
using RovioTest.Config;

namespace RovioTest.Models
{
    public class CourtModel : IDisposable
    {
        private CourtConfig _config;

        public void SetConfig(CourtConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            
        }
    }
}