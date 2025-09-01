using System;
using RovioTest.View;

namespace RovioTest.Models
{
    public class LevelModel : IDisposable
    {
        public CourtView CourtView{ get; private set; }
        public CharacterView PlayerView { get; private set; }
        public CharacterView EnemyView{ get; private set; }
        public BallView BallView{ get; private set; }

        public LevelModel() { }
        public void Dispose() { }
        
        public bool IsLoaded => CourtView != null
                                && PlayerView != null
                                && EnemyView != null
                                && BallView != null;

        public void SetCourtView(CourtView courtView)
        {
            CourtView = courtView;
        }

        public void SetPlayerView(CharacterView playerView)
        {
            PlayerView = playerView;
        }
        public void SetEnemyView(CharacterView enemyView)
        {
            EnemyView = enemyView;
        }
        
        public void SetBallView(BallView ballView)
        {
            BallView = ballView;
        }
    }
}