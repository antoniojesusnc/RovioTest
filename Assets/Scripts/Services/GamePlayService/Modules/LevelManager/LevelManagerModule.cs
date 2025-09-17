using System;
using DG.Tweening;
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
    public class LevelManagerModule : GamePlayModule,
        IEventBusObservable<OnCharacterBeingHitEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>
    {
        public LevelModel LevelModel { get; private set; }
        public bool IsGameOver { get; private set; }
        
        private IGamePlayService _gameplayService;
        private Tween _timerToShowServe;

        public override void Init()
        {
            base.Init();
            _gameplayService = StaticServiceLocator.Get<IGamePlayService>();

            LevelModel = new LevelModel();
        }

        public override void BeginBattle()
        {
            base.BeginBattle();

            IsGameOver = false;
            LoadAssetForBattle();
        }

        private void ResetValues()
        {
            LevelModel = new LevelModel();
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
            var playerView = loadedAsset.GetComponent<CharacterView>();
            playerView.SetModel(playerModel);
            LevelModel.SetPlayerView(playerView);

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
            var enemyView = loadedAsset.GetComponent<CharacterView>();
            enemyView.SetModel(enemyModel);
            LevelModel.SetEnemyView(enemyView);
            
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
            var courtView = loadedAsset.GetComponent<CourtView>();
            courtView.SetModel(courtModel);

            LevelModel.SetCourtView(courtView);
            
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
            var ballView = loadedAsset.GetComponent<BallView>();
            ballView.SetModel(ballModel);

            LevelModel.SetBallView(ballView);
            
            CheckForFinishLoadLevel();
        }

        private void CheckForFinishLoadLevel()
        {
            if (!LevelModel.IsLoaded)
            {
                return;
            }
            
            FinishLoadLevel();
        }

        private void FinishLoadLevel()
        {
            _eventBusService.Send(new OnBeginBattleEvent(LevelModel));
            DOVirtual.DelayedCall(_gameplayService.Config.InitialAnimationDuration, () => BeginServeEvent(LevelModel.PlayerView));
        }
        
        private void BeginServeEvent(CharacterView serverCharacter)
        {
            if (IsGameOver)
            {
                return;
            }
            
            _eventBusService.Send(new OnBeginServeEvent(serverCharacter));
        }

        public override void GameOver(bool isWon)
        {
            IsGameOver = true;
            base.GameOver(isWon);
            _timerToShowServe?.Kill();
        }

        public void OnNewEvent(OnCharacterBeingHitEvent newEvent)
        {
            if (newEvent.Character.Model.IsAlive)
            {
                return;
            }

            bool isWin = newEvent.Character == LevelModel.EnemyView;
            _gameplayService.GameOver(isWin);
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            var server = newEvent.CharacterDown == LevelModel.PlayerView ? LevelModel.EnemyView : LevelModel.PlayerView;
            _timerToShowServe = DOVirtual.DelayedCall(_gameplayService.Config.WaitTimeAfterSmash, () => BeginServeEvent(server));
        }
    }
}