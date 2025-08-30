using System;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using Urd;
using Urd.Services;

namespace RovioTest.Services
{
    [Serializable]
    public class LevelManagerModule : GamePlayModule
    {
        private CourtView _courtView;
        public CharacterView PlayerView { get; private set; }
        private CharacterView _enemyView;
        private BallView _ballView;
        
        private IGamePlayService _gameplayService;

        public override void Init()
        {
            base.Init();
            _gameplayService = StaticServiceLocator.Get<IGamePlayService>();
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
            PlayerView = null;
            _enemyView = null;
        }

        private void LoadAssetForBattle()
        {
            var playerModel = _gameplayService.PlayerModel;
            LoadPlayer(playerModel);
            
            var enemy = ChooseEnemy();
            LoadEnemy(enemy);
            
            var court = ChooseCourt();
            LoadCourt(court);

            var ball = ChooseBall();
            LoadBall(ball);
        }

        private BallConfig ChooseBall()
        {
            return _gameplayService.Config.Balls.GetRandom();
        }

        private CourtConfig ChooseCourt()
        {
            return _gameplayService.Config.Courts.GetRandom();
        }

        private CharacterConfig ChooseEnemy()
        {
            return _gameplayService.Config.Characters.GetRandom();
        }

        private void LoadPlayer(CharacterModel playerModel)
        {
            playerModel.Config.Asset.InstantiateAsync().Completed += loadedAsset => OnLoadPlayer(playerModel, loadedAsset.Result);
        }

        private void OnLoadPlayer(CharacterModel playerModel, GameObject loadedAsset)
        {
            PlayerView = loadedAsset.GetComponent<CharacterView>();
            PlayerView.SetModel(playerModel);

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
        
        private void LoadBall(BallConfig ballConfig)
        {
            ballConfig.Asset.InstantiateAsync().Completed += loadedAsset => OnLoadBall(ballConfig, loadedAsset.Result);
        }

        private void OnLoadBall(BallConfig ballConfig, GameObject loadedAsset)
        {
            var ballModel = new BallModel();
            ballModel.SetConfig(ballConfig);
            _ballView = loadedAsset.GetComponent<BallView>();
            _ballView.SetModel(ballModel);

            CheckForFinishLoadLevel();
        }

        private void CheckForFinishLoadLevel()
        {
            if (_courtView == null
                || PlayerView == null
                || _enemyView == null
                || _ballView == null)
            {
                return;
            }
            
            FinishLoadLevel();
        }

        private void FinishLoadLevel()
        {
            _eventBusService.Send(new OnBeginBattleEvent(_courtView, PlayerView, _enemyView, _ballView));
        }
    }
}