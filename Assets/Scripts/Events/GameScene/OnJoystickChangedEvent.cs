using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnJoystickChangedEvent : IEventBusMessage
    {
        public Vector2 JoystickDelta { get; private set; }
        public bool IsPointerDown { get; private set; }

        public OnJoystickChangedEvent(bool isPointerDown, Vector2 joystickDelta)
        {
            IsPointerDown = isPointerDown;
            JoystickDelta = joystickDelta;
        }
    }
}