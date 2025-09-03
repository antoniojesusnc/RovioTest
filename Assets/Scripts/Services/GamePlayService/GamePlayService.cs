using System;
using System.Collections.Generic;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private IEventBusService _eventBusService;

        public CharacterModel PlayerModel { get; private set; }

        public override void Init()
        {
            base.Init();
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            
            InitModules();
            LoadPlayerData();
        }

        private void LoadPlayerData()
        {
            PlayerModel = new CharacterModel();
            PlayerModel.SetConfig(Config.DefaultCharacterConfig, true);
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
        public void BeginGame()
        {
            SceneManager.LoadScene(SceneUtils.MainMenuSceneIndex);
            StaticServiceLocator.Get<IAudioService>().PlaySound(RovioTestAudiosTypes.MainMenu);
            
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
            StaticServiceLocator.Get<IAudioService>().StopSound(RovioTestAudiosTypes.MainMenu);
            SceneManager.LoadScene(SceneUtils.GameSceneIndex);
            
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.BeginBattle();
            }
        }

        public void GameOver(bool isWon)
        {
            for (int i = 0; i < _gamePlayServiceModule.Count; i++)
            {
                _gamePlayServiceModule[i]?.GameOver(isWon);
            }
            _eventBusService.Send(new OnGameOverEvent(isWon));
        }
    }
}