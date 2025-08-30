using RovioTest.Config;
using UnityEngine;

namespace RovioTest.Models
{
    public class CharacterModel : MonoBehaviour
    {
        private CharacterConfig _config;

        [field: SerializeField] public float CurrentHp { get; private set; }

        public float MaxHP => _config?.Hp ?? 0;
        public float Speed => _config?.Speed ?? 0;

        public void SetConfig(CharacterConfig config)
        {
            _config = config;
        }
    }
}