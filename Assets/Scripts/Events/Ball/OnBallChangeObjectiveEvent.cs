using RovioTest.View;
using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBallChangeObjectiveEvent : IEventBusMessage
    {
        public CharacterView Objetive { get; private set; } 
        public Vector3 Direction { get; private set; }
        public bool SendToPlayer => Objetive != null;

        public OnBallChangeObjectiveEvent(CharacterView objective, Vector3 direction)
        {
            Objetive = objective;
            Direction = direction;
        }
    }
}