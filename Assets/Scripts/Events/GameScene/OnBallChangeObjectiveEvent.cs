using RovioTest.View;
using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBallChangeObjectiveEvent : IEventBusMessage
    {
        public CharacterView Objetive { get; private set; } 
        public Vector3 Position { get; private set; }
        public bool SendToPlayer => Objetive != null;

        public OnBallChangeObjectiveEvent(CharacterView objective) : this(objective, Vector3.zero){}
        public OnBallChangeObjectiveEvent(Vector3 position) : this(null, position){}
        public OnBallChangeObjectiveEvent(CharacterView objective, Vector3 position)
        {
            Objetive = objective;
            Position = position;
        }
    }
}