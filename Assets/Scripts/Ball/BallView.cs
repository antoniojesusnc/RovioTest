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
        IEventBusObservable<OnCharacterDownEvent>
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private TextMeshPro _text;

        private CharacterView _objective;

        public BallModel Model { get; private set; }
        public bool IsMoving { get; private set; }

        public void SetModel(BallModel model)
        {
            Model = model;
        }
        
        private void CustomUpdate(float deltaTime)
        {
            Move(deltaTime);
        }

        private void BeginMovement(CharacterView objective)
        {
            _objective = objective;
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);
        }
        public void Move(float deltaTime)
        {
            var movement = (_objective.transform.position - _rigidBody.position).normalized * Model.Speed* deltaTime;
            _rigidBody.Move(transform.position + movement, Quaternion.identity);
        }
        
        private void Stop()
        {
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
        }

        private void SetScore()
        {
            _text.SetText(Model.CurrentScore.ToString("#0"));
        }
        
        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            SetScore();
            BeginMovement(newEvent.Objetive);
        }

        public void OnNewEvent(OnCharacterDownEvent newEvent)
        {
            Stop();
        }
    }
}