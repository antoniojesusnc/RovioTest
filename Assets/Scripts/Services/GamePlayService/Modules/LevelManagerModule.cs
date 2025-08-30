using System;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Urd;
using Urd.Services;

namespace RovioTest
{
    [Serializable]
    public class LevelManagerModule : GamePlayModule, IEventBusObservable<OnJoystickChangedEvent>
    {
        [SerializeField]
        private LevelManagerConfig _config;
        
        private CourtView _courtView;
        private CharacterView _playerView;
        private CharacterView _enemyView;

        private void MoveCharacter(Vector2 newEventJoystickDelta)
        {
            _playerView.Move(newEventJoystickDelta);
        }

        public override void BeginBattle()
        {
            base.BeginBattle();

            ResetValues();
            
            SceneManager.LoadScene(SceneUtils.GameSceneIndex);
            LoadAssetForBattle();
        }

        private void ResetValues()
        {
            _courtView = null;
            _playerView = null;
            _enemyView = null;
        }

        private void LoadAssetForBattle()
        {
            var playerModel = StaticServiceLocator.Get<IGamePlayService>().PlayerModel;
            LoadPlayer(playerModel);
            var enemy = _config.Characters.GetRandom();
            LoadEnemy(enemy);
            var court = _config.Courts.GetRandom();
            LoadCourt(court);
        }

        private void LoadPlayer(CharacterModel playerModel)
        {
            playerModel.Config.Asset.InstantiateAsync().Completed += loadedAsset => OnLoadPlayer(playerModel, loadedAsset.Result);
        }

        private void OnLoadPlayer(CharacterModel playerModel, GameObject loadedAsset)
        {
            _playerView = loadedAsset.GetComponent<CharacterView>();
            _playerView.SetModel(playerModel);

            CheckForFinishLoadLevel();
        }
        
        private void LoadEnemy(CharacterConfig enemyConfig)
        {
            enemyConfig.Asset.InstantiateAsync().Completed += loadedAsset => OnLoadEnemy(enemyConfig, loadedAsset.Result);
        }

        private void OnLoadEnemy(CharacterConfig enemyConfig, GameObject loadedAsset)
        {
            CharacterModel enemyModel = new CharacterModel();
            enemyModel.SetConfig(enemyConfig);
            _enemyView = loadedAsset.GetComponent<CharacterView>();
            _enemyView.SetModel(enemyModel);

            CheckForFinishLoadLevel();
        }

        private void LoadCourt(CourtConfig courtConfig)
        {
            courtConfig.Asset.InstantiateAsync().Completed += loadedAsset => OnLoadCourt(courtConfig, loadedAsset.Result);
        }

        private void OnLoadCourt(CourtConfig courtConfig, GameObject loadedAsset)
        {
            var courtModel = new CourtModel();
            courtModel.SetConfig(courtConfig);
            _courtView = loadedAsset.GetComponent<CourtView>();
            _courtView.SetModel(courtModel);

            CheckForFinishLoadLevel();
        }

        private void CheckForFinishLoadLevel()
        {
            if (_courtView == null
                || _playerView == null
                || _enemyView == null)
            {
                return;
            }
            
            FinishLoadLevel();
        }

        private void FinishLoadLevel()
        {
            _eventBusService.Send(new OnBeginBattleEvent(_courtView, _playerView, _enemyView));
        }

        public void OnNewEvent(OnJoystickChangedEvent newEvent)
        {
            MoveCharacter(newEvent.joystickDelta);
        }
    }
}