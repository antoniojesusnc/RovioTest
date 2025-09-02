using RovioTest.Events;
using RovioTest.Models;
using UnityEngine;
using Urd;

namespace RovioTest.View
{
    public class CharacterView : MonoBehaviourEventObservable, 
        IEventBusObservable<OnGameOverEvent>
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private SpriteRenderer _hitArea;
        [SerializeField]
        private Transform _skillEffectParent;
        
        private bool _isGameOver;

        public CharacterModel Model { get; private set; }
        public bool IsMoving { get; private set; }

        public void SetModel(CharacterModel model)
        {
            Model = model;

            UpdateData();
        }

        private void UpdateData()
        {
            _hitArea.transform.localScale = Vector3.one * Model.HitRadius;
        }

        public void Move(Vector2 movementNormalized)
        {
            var movement = new Vector3(movementNormalized.x, 0, movementNormalized.y) * Model.Speed* Time.deltaTime;
            _rigidBody.Move(transform.position + movement, Quaternion.identity);
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }

        public void OnBallCollision(Collision collision)
        {
            if (_isGameOver)
            {
                return;
            }

            _eventBusService.Send(OnBallBeingHitEvent.HitWithCharacter(this, collision.GetContact(0)));
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            _isGameOver = true;
        }
    }
}