using RovioTest.Config;
using UnityEngine;

namespace RovioTest.Models
{
    public class CourtModel : MonoBehaviour
    {
        private CourtConfig _config;

        public void SetConfig(CourtConfig config)
        {
            _config = config;
        }
    }
}