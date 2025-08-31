using System;
using MyBox;
using RovioTest.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RovioTest.AI
{
    [Serializable]
    public class CharacterMovementBehaviorRandomMovement : CharacterMovementBehavior
    {
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _movementTime;
        [SerializeField] private float _rateToBeginMovement;
        private Vector2 _direction;
        private float _timeStamp;

        public override void Begin(CharacterView characterView)
        {
            base.Begin(characterView);
            _timeStamp = 0;
        }

        public override void Restart()
        {
            base.Restart();
            _timeStamp = 0;
        }

        protected override bool TryGetMovemenet(out Vector2 movementNormalized)
        {
            if (_timeStamp > 0)
            {
                _timeStamp -= Time.deltaTime;
            }

            if (!IsCurrentActionFinished())
            {
                movementNormalized = _direction;
                return movementNormalized != Vector2.zero;
            }
            
            if (!_isMoving)
            {
                if (NeedToBeginMovement())
                {
                     BeginMovement();
                }
            }
            else
            {
                FinishMovement();
                BeginCooldown();
            }

            movementNormalized = _direction;
            return movementNormalized != Vector2.zero;
        }

        private void BeginCooldown()
        {
            _timeStamp = _cooldownTime;
        }

        private void FinishMovement()
        {
            _direction = Vector2.zero;
        }

        private bool IsCurrentActionFinished()
        {
            return _timeStamp <= 0;
        }

        private void BeginMovement()
        {
            var randomPositionAround = _characterView.transform.position.ToVector2XZ() + Random.insideUnitCircle; 
            _direction = (randomPositionAround - _characterView.transform.position.ToVector2XZ()).normalized;
            _timeStamp = _movementTime;
        }

        private bool NeedToBeginMovement()
        {
            return Random.value < _rateToBeginMovement;
        }
    }
}