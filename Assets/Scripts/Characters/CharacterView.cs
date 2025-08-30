using RovioTest.Models;
using UnityEngine;

namespace RovioTest.View
{
    public class CharacterView : MonoBehaviour
    {
        private CharacterModel _model;
        private Rigidbody _rigidBody;

        public void SetModel(CharacterModel model)
        {
            _model = model;
        }


        public void Move(Vector2 movementDelta)
        {
            var movement = new Vector3(movementDelta.x, 0, movementDelta.y) * _model.Speed* Time.deltaTime;
            _rigidBody.Move(transform.position + movement, Quaternion.identity);
        }
    }
}