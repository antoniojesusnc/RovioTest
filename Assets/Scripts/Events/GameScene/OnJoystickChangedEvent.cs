using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnJoystickChangedEvent : IEventBusMessage
    {
        public Vector2 joystickDelta { get; private set; }

        public OnJoystickChangedEvent(Vector2 joystickDelta)
        {
            this.joystickDelta = joystickDelta;
        }
    }
}