using System;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RovioTest.AI
{
    [Serializable]
    public class CharacterMovementBehaviorQuiet : CharacterMovementBehavior
    {
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _movementTime;
        [SerializeField] private float _rateToBeginMovement;
        private Vector2 _direction;
        private float _timeStamp;

        protected override bool TryGetMovemenet(out Vector2 movementNormalized)
        {
            if (_timeStamp > 0)
            {
                _timeStamp-= Time.deltaTime;
            } 
                
            if (!_isMoving && _timeStamp < 0 )
            {
                if (NeedToBeginMovement())
                {
                     AssignMovement();
                     _timeStamp = _movementTime;
                }
            }
            else if (IsMovementFinished())
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

        private bool IsMovementFinished()
        {
            return _timeStamp <= 0;
        }

        private void AssignMovement()
        {
            _direction = (Random.insideUnitCircle - _characterView.transform.position.ToVector2XZ()).normalized;
        }

        private bool NeedToBeginMovement()
        {
            return Random.value < _rateToBeginMovement;
        }
    }
}