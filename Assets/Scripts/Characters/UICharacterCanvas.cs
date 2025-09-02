using System;
using UnityEngine;
using Urd.Services;

namespace RovioTest.UI
{
    public class UICharacterCanvas : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);
        }
        
        private void OnDisable()
        {
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
        }

        private void CustomUpdate(float deltaTime)
        {
            transform.LookAt(_camera.transform.position);
        }
    }
}