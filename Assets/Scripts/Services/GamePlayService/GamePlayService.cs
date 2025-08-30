using System;
using System.Collections.Generic;
using RovioTest.Models;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    [Serializable]
    public class GamePlayService : BaseService, IGamePlayService
    {
        public override int LoadPriority => ServicesPriority.Lowest;
        
        [field: SerializeField]
        public GamePlayConfig Config { get; private set; }
        
        [SerializeReference, SubclassSelector]
        private List<IGamePlayModule> _gamePlayServiceModule;

        public CharacterModel PlayerModel { get; private set; }

        public override void Init()
        {
            base.Init();
            InitModules();
            LoadPlayerData();
            BeginGame();
        }

        private void LoadPlayerData()
        {
            PlayerModel = new CharacterModel();
            PlayerModel.SetConfig(Config.DefaultCharacterConfig);
        }

        private void InitModules()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i].Init();
            }
        }
        public override void Dispose()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.Dispose();
            }
            base.Dispose();
        }
        private void BeginGame()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.BeginGame();
            }
        }

        public T GetModule<T>() where T : class, IGamePlayModule
        {
            return _gamePlayServiceModule.Find(module => typeof(T).IsAssignableFrom(module.GetType())) as T;
        }

        public void BeginBattle()
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.BeginBattle();
            }
        }
    }
}