using System;
using System.Collections.Generic;
using RovioTest.Models;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest
{
    [Serializable]
    public class GamePlayService : BaseService
    {
        public override int LoadPriority => ServicesPriority.Lowest;
        
        [SerializeReference, SubclassSelector]
        private List<IGamePlayModule> _gamePlayServiceModule;

        public CharacterModel PlayerModel { get; private set; }
        
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
    }
}