using System;
using RovioTest.Config;
using UnityEngine;

namespace RovioTest.Models
{
    public class CharacterModel : IDisposable
    {
        public CharacterConfig Config { get; private set; }

        [field: SerializeField] public float CurrentHp { get; private set; }

        public float MaxHP => Config?.Hp ?? 0;
        public float Speed => Config?.Speed ?? 0;

        public void Dispose()
        {
            
        }
        
        public void SetConfig(CharacterConfig config)
        {
            Config = config;
        }

    }
}