using RovioTest.Models;
using UnityEngine;

namespace RovioTest.View
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private SpriteRenderer _hitArea;
        
        public CharacterModel Model { get; private set; }
        public bool IsMoving { get; private set; }

        public void SetModel(CharacterModel model)
        {
            Model = model;

            UpdateData();
        }

        private void UpdateData()
        {
            _hitArea.transform.localScale = Vector3.one * Model.HitRadius;
        }

        public void Move(Vector2 movementNormalized)
        {
            var movement = new Vector3(movementNormalized.x, 0, movementNormalized.y) * Model.Speed* Time.deltaTime;
            _rigidBody.Move(transform.position + movement, Quaternion.identity);
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }
    }
}