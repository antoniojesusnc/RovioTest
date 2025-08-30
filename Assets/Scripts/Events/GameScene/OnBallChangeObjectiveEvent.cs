using RovioTest.View;
using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBallChangeObjectiveEvent : IEventBusMessage
    {
        public CharacterView Objetive { get; private set; } 
        public Vector3 Position { get; private set; } 
        public bool SendToPlayer { get; private set; }

        public OnBallChangeObjectiveEvent(CharacterView objective) : this(objective, Vector3.zero, true){}
        public OnBallChangeObjectiveEvent(Vector3 position) : this(null, position, false){}
        public OnBallChangeObjectiveEvent(CharacterView objective, Vector3 position, bool sendToPlayer)
        {
            Objetive = objective;
            Position = position;
            SendToPlayer = sendToPlayer;
        }
    }
}