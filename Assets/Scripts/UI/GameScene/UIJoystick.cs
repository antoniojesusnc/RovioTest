using System;
using RovioTest.Events;
using UnityEngine;
using Urd;
using zFrame.UI;

namespace RovioTest.UI
{
    [Serializable]
    public class UIJoystick : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnGameOverEvent>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private Joystick _joystick;

        bool _isPointerDown = false;

        protected override void Start()
        {
            base.Start();
        }
        
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _joystick.OnValueChanged.AddListener(OnJoystickChanged);
            _joystick.OnPointerUp.AddListener(OnPointerUp);
            _joystick.OnPointerDown.AddListener(OnPointerDown);
        }

        private void GameOver()
        {
            gameObject.SetActive(false);
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
            _joystick?.OnValueChanged.RemoveAllListeners();
            _joystick?.OnPointerUp.RemoveAllListeners();
            _joystick?.OnPointerDown.RemoveAllListeners();
        }

        public void OnJoystickChanged(Vector2 joystickDelta)
        {
            if (!_joystick.IsDraging)
            {
                return;
            }
            
            MoveCharacter(joystickDelta);
        }
        
        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            Hide();
        }
        
        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            GameOver();
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            Hide();
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            Show();
        }
    }
}