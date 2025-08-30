using RovioTest.Models;
using UnityEngine;

namespace RovioTest.View
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        
        public CharacterModel Model { get; private set; }
        public bool IsMoving { get; private set; }


        public void SetModel(CharacterModel model)
        {
            Model = model;
        }

        public void Move(Vector2 movementDelta)
        {
            var movement = new Vector3(movementDelta.x, 0, movementDelta.y) * Model.Speed* Time.deltaTime;
            _rigidBody.Move(transform.position + movement, Quaternion.identity);
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }
    }
}