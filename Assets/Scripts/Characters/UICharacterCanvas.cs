using UnityEngine;

namespace RovioTest.UI
{
    public class UICharacterCanvas : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            transform.LookAt(-_camera.transform.position);
        }
    }
}