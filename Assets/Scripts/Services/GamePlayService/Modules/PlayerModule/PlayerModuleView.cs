using System;
using RovioTest.Events;
using UnityEngine;
using Urd;
using zFrame.UI;

namespace RovioTest
{
    [Serializable]
    public class PlayerModuleView : MonoBehaviourEventObservable
    {
        private Joystick _joystick;

        bool _isPointerDown = false;
        protected override void Start()
        {
            base.Start();
            _joystick = FindFirstObjectByType<Joystick>();
            _joystick?.OnValueChanged.AddListener(OnJoystickChanged);
            _joystick?.OnPointerUp.AddListener(OnPointerUp);
            _joystick?.OnPointerDown.AddListener(OnPointerDown);
        }

        private void OnPointerDown(Vector2 joystickDelta)
        {
            _isPointerDown = true;
            MoveCharacter(joystickDelta);
        }

        private void OnPointerUp(Vector2 joystickDelta)
        {
            _isPointerDown = false;
            MoveCharacter(joystickDelta);
        }

        private void MoveCharacter(Vector2 joystickDelta)
        {
            _eventBusService.Send(new OnJoystickChangedEvent(_isPointerDown, joystickDelta));
        }

        private void OnDisable()
        {
            _joystick?.OnValueChanged.RemoveListener(OnJoystickChanged);
        }

        public void OnJoystickChanged(Vector2 joystickDelta)
        {
            if (!_joystick.IsDraging)
            {
                return;
            }
            
            MoveCharacter(joystickDelta);
        }
    }
}