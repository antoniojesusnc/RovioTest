using RovioTest.Events;
using UnityEngine;
using Urd;

namespace RovioTest.UI
{
    public class UICharacterCanvas : MonoBehaviourEventObservable,
        IEventBusObservable<OnFinishInitialCameraAnimation>
    {
        private Camera _camera;

        protected override void Start()
        {
            base.Start();
            _camera = Camera.main;
            
        }

        public void OnNewEvent(OnFinishInitialCameraAnimation newEvent)
        {
            transform.LookAt(-_camera.transform.position);
        }
    }
}