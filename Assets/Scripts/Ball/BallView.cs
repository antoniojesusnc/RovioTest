using MyBox;
using RovioTest.Events;
using RovioTest.Models;
using TMPro;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.View
{
    public class BallView : MonoBehaviourEventObservable, 
        IEventBusObservable<OnBallChangeObjectiveEvent>,
        IEventBusObservable<OnCharacterDownEvent>,
        IEventBusObservable<OnGameOverEvent>
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private TextMeshPro _text;
        
        private CharacterView _objective;

        public BallModel Model { get; private set; }
        public bool IsMoving { get; private set; }

        protected override void Start()
        {
            base.Start();
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);
        }

        public void SetModel(BallModel model)
        {
            Model = model;
        }
        
        private void CustomUpdate(float deltaTime)
        {
            Move(deltaTime);
        }

        private void BeginMovement(CharacterView objective, Vector3 direction)
        {
            _objective = objective;
            transform.LookAt(transform.position + direction.SetY(0));
            _rigidBody.rotation = transform.rotation;
            
            IsMoving = true;
        }
        public void Move(float deltaTime)
        {
            if (!IsMoving)
            {
                return;
            }
            
            var direction = (_objective.transform.position - _rigidBody.position).normalized;
            var step = Model.MaxTurnDegreesAngle * Mathf.Deg2Rad * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0);
            transform.LookAt(transform.position + newDirection.SetY(0));
            _rigidBody.rotation = transform.rotation;
            
            var movement = transform.forward.normalized * Model.Speed* deltaTime;
            _rigidBody.MovePosition(transform.position + movement);
        }
        
        private void Stop()
        {
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
            IsMoving = false;
        }

        private void SetScore()
        {
            _text.SetText(Model.CurrentScore.ToString("#0"));
        }
        
        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            SetScore();
            BeginMovement(newEvent.Objetive, newEvent.Direction);
        }

        public void OnNewEvent(OnCharacterDownEvent newEvent)
        {
            Stop();
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            Stop();
        }
    }
}