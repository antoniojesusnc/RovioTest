using System;
using RovioTest.Config;

namespace RovioTest.Models
{
    public class BallModel : IDisposable
    {
        public BallConfig Config { get; private set; }

        public float InitialSpeed => Config?.Speed ?? 0;
        public float Speed { get; private set; }
        public float Hits { get; private set; }
        public int CurrentScore { get; private set; }
        public float MaxTurnDegreesAngle => Config.MaxTurnDegreesAngle;

        public void Dispose()
        {
            
        }
        
        public void SetConfig(BallConfig config)
        {
            Config = config;
        }

        public void SetScore(int score)
        {
            CurrentScore = score;
        }
        
        public void AddScore(int score)
        {
            CurrentScore += score;
        }

        public void BeginMovement()
        {
            ResetToInitialSpeed();
            Hits = 0;
        }

        public void Hit()
        {
            Hits++;
        }

        public void ResetToInitialSpeed()
        {
            Speed = InitialSpeed;
        }
        public void IncreaseSpeedRate(float increaseRate)
        {
            Speed *= increaseRate;
        }
    }
}